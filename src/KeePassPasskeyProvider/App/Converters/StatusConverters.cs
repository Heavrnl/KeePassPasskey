// SPDX-FileCopyrightText: Copyright (C) 2026 Uwe Koegel
// SPDX-License-Identifier: GPL-3.0-or-later
using Avalonia.Data.Converters;
using KeePassPasskeyShared.Ipc;
using KeePassPasskeyShared.Settings;
using KeePassPasskeyProvider.App.ViewModel;

namespace KeePassPasskeyProvider.App.Converters;

internal static class ProviderStatusConverters
{
	public static readonly IValueConverter Headline = Make(v => (v as ProviderStatus? ?? ProviderStatus.NotRegistered) switch
	{
		ProviderStatus.AutoregisterFailed => Util.LocalizationService.Instance.IsChinese ? "自动注册失败" : "Automatic registration failed",
		ProviderStatus.NotRegistered => Util.LocalizationService.Instance.IsChinese ? "未注册" : "Not registered",
		ProviderStatus.WaitingToBeEnabled => Util.LocalizationService.Instance.IsChinese ? "已注册，等待在系统设置中启用" : "Waiting to be enabled",
		ProviderStatus.IncompatibleVersion => Util.LocalizationService.Instance.IsChinese ? "版本不兼容" : "Incompatible version",
		ProviderStatus.VersionMismatch => Util.LocalizationService.Instance.IsChinese ? "版本不匹配" : "Version mismatch",
		ProviderStatus.NoDatabase => Util.LocalizationService.Instance.IsChinese ? "未解锁任何数据库" : "No database open",
		ProviderStatus.KeePassNotConnected => Util.LocalizationService.Instance.IsChinese ? "KeePass 未连接" : "KeePass not connected",
		_ => Util.LocalizationService.Instance.IsChinese ? "所有系统已就绪" : "All systems ready",
	});

	public static readonly IValueConverter ProviderPillText = Make(v => (v as ProviderStatus? ?? ProviderStatus.NotRegistered) switch
	{
		ProviderStatus.Ready or ProviderStatus.KeePassNotConnected or ProviderStatus.NoDatabase or
		ProviderStatus.VersionMismatch or ProviderStatus.IncompatibleVersion => Util.LocalizationService.T("Enabled"),
		ProviderStatus.WaitingToBeEnabled => Util.LocalizationService.T("Registered"),
		_ => Util.LocalizationService.T("NotRegistered"),
	});

	private static LambdaConverter Make(Func<object?, object?> convert) => new(convert);
}

internal static class PluginPillConverters
{
	public static readonly IValueConverter Text = Make(v => (v as PluginPillState? ?? PluginPillState.NotConnected) switch
	{
		PluginPillState.Running => Util.LocalizationService.T("Running"),
		PluginPillState.NoDatabase => Util.LocalizationService.Instance.IsChinese ? "未打开数据库" : "No database open",
		PluginPillState.VersionMismatch => Util.LocalizationService.Instance.IsChinese ? "版本不匹配" : "Version mismatch",
		PluginPillState.IncompatibleVersion => Util.LocalizationService.Instance.IsChinese ? "版本不兼容" : "Incompatible version",
		_ => Util.LocalizationService.T("NotDetected"),
	});

	private static LambdaConverter Make(Func<object?, object?> convert) => new(convert);
}
