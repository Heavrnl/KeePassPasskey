// SPDX-FileCopyrightText: Copyright (C) 2026 Uwe Koegel
// SPDX-License-Identifier: GPL-3.0-or-later
using Avalonia.Data;
using Avalonia.Markup.Xaml;

namespace KeePassPasskeyProvider.Util;

public class TrExtension : MarkupExtension
{
	public string Key { get; set; } = string.Empty;

	public TrExtension()
	{
	}

	public TrExtension(string key)
	{
		Key = key;
	}

	public override object ProvideValue(IServiceProvider serviceProvider)
	{
		return new Binding($"[{Key}]")
		{
			Source = LocalizationService.Instance,
			Mode = BindingMode.OneWay,
		};
	}
}
