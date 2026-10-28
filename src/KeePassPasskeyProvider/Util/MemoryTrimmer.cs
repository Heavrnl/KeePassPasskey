// SPDX-FileCopyrightText: Copyright (C) 2026 Uwe Koegel
// SPDX-License-Identifier: GPL-3.0-or-later
using System.Diagnostics;
using KeePassPasskeyShared;

namespace KeePassPasskeyProvider.Util;

/// <summary>
/// Proactively trims process memory and working set when idle or after ceremonies/UI transitions.
/// </summary>
internal static class MemoryTrimmer
{
	private static readonly Lock _lock = new();
	private static CancellationTokenSource? _scheduledTrimCts;

	/// <summary>
	/// Immediately collects garbage and trims the process working set.
	/// </summary>
	internal static void TrimNow()
	{
		try
		{
			// Aggressive GC collect and compaction
			GC.Collect(GC.MaxGeneration, GCCollectionMode.Aggressive, blocking: true, compacting: true);
			GC.WaitForPendingFinalizers();
			GC.Collect(GC.MaxGeneration, GCCollectionMode.Aggressive, blocking: true, compacting: true);

			// Release unneeded physical memory pages back to Windows
			nint handle = Process.GetCurrentProcess().Handle;
			if (handle != 0)
			{
				Win32Native.EmptyWorkingSet(handle);
			}
			Log.Debug($"Working set trimmed. Current WS: {Process.GetCurrentProcess().WorkingSet64 / 1024 / 1024} MB", nameof(MemoryTrimmer));
		}
		catch (Exception ex)
		{
			Log.Debug($"TrimNow failed: {ex.Message}", nameof(MemoryTrimmer));
		}
	}

	/// <summary>
	/// Schedules a trim after a short delay (e.g. 500ms~1000ms), coalescing rapid calls.
	/// </summary>
	internal static void ScheduleTrim(int delayMs = 1000)
	{
		lock (_lock)
		{
			_scheduledTrimCts?.Cancel();
			_scheduledTrimCts?.Dispose();
			var cts = new CancellationTokenSource();
			_scheduledTrimCts = cts;

			_ = Task.Run(async () =>
			{
				try
				{
					await Task.Delay(delayMs, cts.Token);
					if (!cts.Token.IsCancellationRequested)
					{
						TrimNow();
					}
				}
				catch (OperationCanceledException) { }
				catch (Exception ex)
				{
					Log.Debug($"Scheduled trim error: {ex.Message}", nameof(MemoryTrimmer));
				}
			});
		}
	}
}
