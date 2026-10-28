// SPDX-FileCopyrightText: Copyright (C) 2026 Uwe Koegel
// SPDX-License-Identifier: GPL-3.0-or-later
using Avalonia.Data.Converters;
using KeePassPasskeyShared.Settings;

namespace KeePassPasskeyProvider.App.Converters;

internal static class ExcludeCredentialCheckModeConverters
{
	public static readonly IValueConverter Text = new LambdaConverter(v => (v as ExcludeCredentialCheckMode?) switch
	{
		ExcludeCredentialCheckMode.None => Util.LocalizationService.Instance.IsChinese ? "不检查" : "Don't check",
		ExcludeCredentialCheckMode.AllDatabases => Util.LocalizationService.Instance.IsChinese ? "检查所有数据库" : "Check all databases",
		_ => Util.LocalizationService.Instance.IsChinese ? "仅检查目标数据库" : "Check target database",
	});
}
