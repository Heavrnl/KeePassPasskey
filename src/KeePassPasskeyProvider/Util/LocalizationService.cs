// SPDX-FileCopyrightText: Copyright (C) 2026 Uwe Koegel
// SPDX-License-Identifier: GPL-3.0-or-later
using System.ComponentModel;
using System.Globalization;

namespace KeePassPasskeyProvider.Util;

internal sealed class LocalizationService : INotifyPropertyChanged
{
	public static LocalizationService Instance { get; } = new();

	public event PropertyChangedEventHandler? PropertyChanged;

	private string _currentLanguage = "System";

	public string CurrentLanguage
	{
		get => _currentLanguage;
		set
		{
			if (_currentLanguage != value)
			{
				_currentLanguage = value;
				UpdateResolvedLanguage();
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Item[]"));
			}
		}
	}

	public bool IsChinese { get; private set; }

	private void UpdateResolvedLanguage()
	{
		if (_currentLanguage.Equals("zh-CN", StringComparison.OrdinalIgnoreCase) ||
		    _currentLanguage.Equals("Chinese", StringComparison.OrdinalIgnoreCase) ||
		    _currentLanguage.Equals("简体中文", StringComparison.OrdinalIgnoreCase))
		{
			IsChinese = true;
		}
		else if (_currentLanguage.Equals("en-US", StringComparison.OrdinalIgnoreCase) ||
		         _currentLanguage.Equals("English", StringComparison.OrdinalIgnoreCase))
		{
			IsChinese = false;
		}
		else // System
		{
			var culture = CultureInfo.InstalledUICulture;
			IsChinese = culture.Name.StartsWith("zh", StringComparison.OrdinalIgnoreCase);
		}
	}

	private LocalizationService()
	{
		UpdateResolvedLanguage();
	}

	public string this[string key] => Get(key);

	public string Get(string key)
	{
		if (IsChinese && StringsZh.TryGetValue(key, out var zh))
			return zh;
		if (StringsEn.TryGetValue(key, out var en))
			return en;
		return key;
	}

	public static string T(string key) => Instance.Get(key);

	private static readonly Dictionary<string, string> StringsZh = new(StringComparer.OrdinalIgnoreCase)
	{
		// 通用与常用操作
		{ "Save", "保存" },
		{ "Cancel", "取消" },
		{ "Discard", "放弃" },
		{ "Apply", "应用" },
		{ "Copy", "复制" },
		{ "Retry", "重试" },
		{ "Reset", "重置" },
		{ "ResetToDefaults", "恢复默认值" },
		{ "OK", "确定" },
		{ "Search", "搜索" },
		{ "SearchEntries", "搜索条目…" },
		{ "Loading", "正在加载…" },
		{ "LoadingSettings", "正在加载设置…" },
		{ "Enabled", "已启用" },
		{ "Disabled", "已禁用" },
		{ "Running", "运行中" },
		{ "NotDetected", "未检测到" },
		{ "Registered", "已注册" },
		{ "NotRegistered", "未注册" },
		{ "Ready", "就绪" },

		// 导航栏
		{ "NavStatus", "运行状态" },
		{ "NavDiagnostics", "诊断信息" },
		{ "NavSettings", "选项设置" },
		{ "DevBuildTitle", "开发调试版本" },
		{ "DevBuildMessage", "这是具有独立标识的开发版本，与正式版并存，不建议用于生产环境。" },
		{ "NotPackagedTitle", "未作为应用包安装" },
		{ "NotPackagedMessage", "注册与注销可能无法正常工作，请先安装 MSIX 应用包。" },

		// 保存密钥弹窗 (Registration Prompt)
		{ "SavePasskeyTitle", "保存通行密钥" },
		{ "WantsToSavePasskey", "希望保存通行密钥" },
		{ "Database", "数据库" },
		{ "CreateNew", "新建条目" },
		{ "AddToExisting", "保存到已有条目" },
		{ "NewEntryNotice", "将为该网站自动创建一个新条目。" },
		{ "AttestationHint", "该网站请求了企业证明，KeePassPasskey 暂不支持该证明，但通常仍可正常使用通行密钥。" },

		// 登录弹窗 (SignIn Prompt)
		{ "SignInTitle", "使用密钥登录" },
		{ "WantsToSignIn", "请求使用通行密钥登录" },
		{ "Allow", "允许" },
		{ "Confirm", "确认" },

		// 主页 (HomePage / SetupGuide)
		{ "StatusPasskeyProvider", "通行密钥提供程序" },
		{ "StatusKeePassPlugin", "KeePass 管道服务" },
		{ "RefreshStatus", "刷新状态" },
		{ "Register", "注册提供程序" },
		{ "Unregister", "注销提供程序" },
		{ "AdvancedPasskeyOptions", "Windows 通行密钥设置" },
		{ "GettingStarted", "新手配置指引" },
		{ "Step1Title", "1. 启动 KeePassXC" },
		{ "Step1Desc", "确保 KeePassXC 保持运行且密码数据库已解锁。" },
		{ "Step2Title", "2. 打开系统高级选项" },
		{ "Step2Desc", "前往 Windows“设置 > 账户 > 通行密钥 > 高级选项”。" },
		{ "Step3Title", "3. 启用 KeePassPasskey" },
		{ "Step3Desc", "在通行密钥提供程序列表中开启 KeePassPasskey。" },
		{ "Step4Title", "4. 开始使用" },
		{ "Step4Desc", "在浏览器中访问任意支持通行密钥的网站进行注册或登录。" },

		// 设置页 (SettingsPage)
		{ "SettingsTitle", "选项设置" },
		{ "UnsavedChangesTooltip", "您有未保存的设置更改" },
		{ "SaveSettingsTooltip", "保存设置" },
		{ "DiscardChangesTooltip", "放弃更改" },
		{ "ResetDefaultsTooltip", "恢复默认值" },
		{ "LoadSettingsFailed", "无法从 KeePass 读取最新设置，当前显示上次缓存的值。" },
		{ "HeaderAppearance", "外观与语言" },
		{ "DescAppearance", "应用程序主题外观与显示语言" },
		{ "Theme", "界面主题" },
		{ "Language", "显示语言" },
		{ "LanguageSystem", "跟随系统语言" },
		{ "LanguageZh", "简体中文" },
		{ "LanguageEn", "English" },

		// 验证模式
		{ "HeaderVerification", "通知与用户验证" },
		{ "DescVerification", "通行密钥操作时的确认提示与用户身份验证方式" },
		{ "ErrorNotifications", "错误通知" },
		{ "ErrorNotificationsDesc", "当操作失败时显示系统通知" },
		{ "RegistrationVerification", "保存新密钥时的审批" },
		{ "RegistrationVerificationDesc", "新建并保存通行密钥时的安全审批机制" },
		{ "SignInVerification", "登录使用密钥时的审批" },
		{ "SignInVerificationDesc", "网站请求使用通行密钥登录时的审批机制" },
		{ "WindowsHelloToggle", "Windows Hello 验证 (PIN、指纹或面容)" },
		{ "PromptToggle", "确认弹窗卡片" },
		{ "SilentModeWarning", "两种验证均已关闭；操作将以完全静默方式自动处理！" },
		{ "SkipHelloAfterUnlock", "解锁密码库后跳过 Windows Hello" },
		{ "SkipHelloAfterUnlockDesc", "若刚为本次请求解锁了密码库，不再重复要求 Windows Hello 验证" },

		// 存储选项
		{ "HeaderDatabase", "凭据与存储选项" },
		{ "DescDatabase", "通行密钥在 KeePass 中的存储方式与条目组织" },
		{ "SaveToExistingEntry", "优先匹配并更新已有条目" },
		{ "SaveToExistingEntryDesc", "若已有条目匹配该网站与用户名，优先将通行密钥添加到该条目" },
		{ "AddPasskeyTag", "自动添加“Passkey”标签" },
		{ "AddPasskeyTagDesc", "为新建的通行密钥条目自动标记 Passkey 标签" },
		{ "EntryTitleTemplate", "新条目标题模板" },
		{ "EntryTitleTemplateDesc", "新建通行密钥条目时的默认标题格式" },
		{ "ResolveTitlePlaceholders", "解析标题占位符" },
		{ "ResolveTitlePlaceholdersDesc", "将 {RP_NAME} 等占位符替换为网站实际名称" },
		{ "NewEntryGroupMode", "新建条目的存放分组" },
		{ "ExcludeCredentialCheckMode", "排除凭据检查范围" },

		// 关于与信息
		{ "HeaderAbout", "关于" },
		{ "DescAbout", "版本信息、源代码与开源许可证" },
		{ "Copyright", "版权信息" },
		{ "Version", "版本" },
		{ "Website", "官方主页" },
		{ "SourceCode", "源代码仓库" },
		{ "Contributors", "贡献者列表" },
		{ "ReportBug", "反馈问题" },
		{ "License", "开源许可证" },
		{ "ThirdPartyNotices", "第三方开源组件声明" },
	};

	private static readonly Dictionary<string, string> StringsEn = new(StringComparer.OrdinalIgnoreCase)
	{
		{ "Save", "Save" },
		{ "Cancel", "Cancel" },
		{ "Discard", "Discard" },
		{ "Apply", "Apply" },
		{ "Copy", "Copy" },
		{ "Retry", "Retry" },
		{ "Reset", "Reset" },
		{ "ResetToDefaults", "Reset to defaults" },
		{ "OK", "OK" },
		{ "Search", "Search" },
		{ "SearchEntries", "Search entries…" },
		{ "Loading", "Loading…" },
		{ "LoadingSettings", "Loading settings…" },
		{ "Enabled", "Enabled" },
		{ "Disabled", "Disabled" },
		{ "Running", "Running" },
		{ "NotDetected", "Not detected" },
		{ "Registered", "Registered" },
		{ "NotRegistered", "Not registered" },
		{ "Ready", "Ready" },

		{ "NavStatus", "Status" },
		{ "NavDiagnostics", "Diagnostics" },
		{ "NavSettings", "Settings" },
		{ "DevBuildTitle", "Development build" },
		{ "DevBuildMessage", "This is a dev build with a separate identity, installed alongside any stable release. Not for production use." },
		{ "NotPackagedTitle", "Not installed as package" },
		{ "NotPackagedMessage", "Register and Unregister will not work correctly. Please install the MSIX package first." },

		{ "SavePasskeyTitle", "Save passkey" },
		{ "WantsToSavePasskey", "wants to save a passkey" },
		{ "Database", "Database" },
		{ "CreateNew", "Create new" },
		{ "AddToExisting", "Add to existing" },
		{ "NewEntryNotice", "A new entry will be created for this site." },
		{ "AttestationHint", "This site asked for enterprise attestation, which KeePassPasskey cannot provide. It may still accept the passkey." },

		{ "SignInTitle", "Sign in with passkey" },
		{ "WantsToSignIn", "wants you to sign in with a passkey" },
		{ "Allow", "Allow" },
		{ "Confirm", "Confirm" },

		{ "StatusPasskeyProvider", "Passkey provider" },
		{ "StatusKeePassPlugin", "KeePass plugin" },
		{ "RefreshStatus", "Refresh status" },
		{ "Register", "Register" },
		{ "Unregister", "Unregister" },
		{ "AdvancedPasskeyOptions", "Advanced passkey settings" },
		{ "GettingStarted", "Getting started" },
		{ "Step1Title", "1. Launch KeePassXC" },
		{ "Step1Desc", "Keep KeePassXC running with your database unlocked." },
		{ "Step2Title", "2. Open Advanced Passkey Options" },
		{ "Step2Desc", "Go to Settings > Accounts > Passkeys > Advanced options." },
		{ "Step3Title", "3. Enable KeePassPasskey" },
		{ "Step3Desc", "Toggle KeePassPasskey on in the passkey provider list." },
		{ "Step4Title", "4. Verify & use" },
		{ "Step4Desc", "Visit any passkey-enabled website in your browser." },

		{ "SettingsTitle", "Settings" },
		{ "UnsavedChangesTooltip", "You have unsaved changes" },
		{ "SaveSettingsTooltip", "Save settings" },
		{ "DiscardChangesTooltip", "Discard changes" },
		{ "ResetDefaultsTooltip", "Reset to defaults" },
		{ "LoadSettingsFailed", "Could not load the latest settings from KeePass. Showing the last known values." },
		{ "HeaderAppearance", "Appearance & Language" },
		{ "DescAppearance", "Application appearance, theme and language" },
		{ "Theme", "Theme" },
		{ "Language", "Display Language" },
		{ "LanguageSystem", "System Default" },
		{ "LanguageZh", "简体中文" },
		{ "LanguageEn", "English" },

		{ "HeaderVerification", "Notifications & User Verification" },
		{ "DescVerification", "Confirmation prompts and user verification for passkey operations" },
		{ "ErrorNotifications", "Error notifications" },
		{ "ErrorNotificationsDesc", "Show a system notification when an operation fails" },
		{ "RegistrationVerification", "Approval for saving a new passkey" },
		{ "RegistrationVerificationDesc", "How you approve saving a new passkey" },
		{ "SignInVerification", "Approval for sign-in" },
		{ "SignInVerificationDesc", "How you approve using a passkey to sign in" },
		{ "WindowsHelloToggle", "Windows Hello (PIN, fingerprint, or face)" },
		{ "PromptToggle", "Confirmation prompt" },
		{ "SilentModeWarning", "Neither prompt nor Windows Hello is enabled; operations will proceed completely silently!" },
		{ "SkipHelloAfterUnlock", "Skip Windows Hello after unlocking KeePass" },
		{ "SkipHelloAfterUnlockDesc", "When KeePass was locked and you just unlocked it for this request, don't ask for Windows Hello again." },

		{ "HeaderDatabase", "Credentials & Storage" },
		{ "DescDatabase", "How passkeys are stored and organized in KeePass" },
		{ "SaveToExistingEntry", "Save to matching entry" },
		{ "SaveToExistingEntryDesc", "When an existing entry matches the site and username, add the passkey to that entry" },
		{ "AddPasskeyTag", "Add 'Passkey' tag" },
		{ "AddPasskeyTagDesc", "Tag newly created passkey entries with 'Passkey'" },
		{ "EntryTitleTemplate", "New entry title template" },
		{ "EntryTitleTemplateDesc", "Format for newly created passkey entry titles" },
		{ "ResolveTitlePlaceholders", "Resolve title placeholders" },
		{ "ResolveTitlePlaceholdersDesc", "Replace {RP_NAME} and other placeholders with actual values" },
		{ "NewEntryGroupMode", "Target group for new entries" },
		{ "ExcludeCredentialCheckMode", "Exclude-credential check scope" },

		{ "HeaderAbout", "About" },
		{ "DescAbout", "Version, source code and license" },
		{ "Copyright", "Copyright" },
		{ "Version", "Version" },
		{ "Website", "Website" },
		{ "SourceCode", "Source code" },
		{ "Contributors", "Contributors" },
		{ "ReportBug", "Report a bug" },
		{ "License", "License" },
		{ "ThirdPartyNotices", "Third-party notices" },
	};
}
