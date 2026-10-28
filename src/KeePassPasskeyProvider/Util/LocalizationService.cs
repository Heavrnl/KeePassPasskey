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
		{ "NotConnected", "未连接" },
		{ "NotDetected", "未检测到" },
		{ "Registered", "已注册" },
		{ "NotRegistered", "未注册" },
		{ "Ready", "就绪" },
		{ "Browse", "浏览…" },

		// 导航栏与警告
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
		{ "ReplaceWarning", "该条目已存在通行密钥，保存将会覆盖现有密钥。" },

		// 登录弹窗 (SignIn Prompt)
		{ "SignInTitle", "使用密钥登录" },
		{ "WantsToSignIn", "请求使用通行密钥登录" },
		{ "Allow", "允许" },
		{ "Confirm", "确认" },

		// 主页 (HomePage / StatusHero / SetupGuide)
		{ "StatusPasskeyProvider", "通行密钥提供程序" },
		{ "StatusKeePassPlugin", "KeePass 管道服务" },
		{ "RefreshStatus", "刷新状态" },
		{ "Register", "注册提供程序" },
		{ "Unregister", "注销提供程序" },
		{ "AdvancedPasskeyOptions", "Windows 通行密钥设置" },
		{ "GettingStarted", "新手配置指引" },
		{ "OpenUserGuide", "打开用户使用指南" },
		{ "Step1Title", "1. 部署与连接 KeePass 插件/内置服务" },
		{ "Step1Desc", "确保 KeePassXC 保持运行，或将插件放置于 KeePass 插件目录。" },
		{ "InstallPlugin", "安装插件" },
		{ "InstallPluginTooltip", "安装或更新 KeePass 插件" },
		{ "ShowPluginFile", "查看插件文件" },
		{ "ShowPluginFileTooltip", "在文件资源管理器中定位 KeePassPasskey.dll" },
		{ "Step2Title", "2. 打开系统高级选项" },
		{ "Step2Desc", "前往 Windows“设置 > 账户 > 通行密钥 > 高级选项”，或点击下方快捷按钮。" },
		{ "Step3Title", "3. 启用 KeePassPasskey" },
		{ "Step3Desc", "在系统通行密钥提供程序列表中将 KeePassPasskey 切换为开启。" },
		{ "Step4Title", "4. 验证就绪" },
		{ "Step4Desc", "上方两项状态均应显示为“已启用 / 运行中”，此时即可在浏览器中畅享通行密钥。" },
		{ "TrayOfferTitle", "想随时在后台留意状态？" },
		{ "TrayOfferDesc", "将 KeePassPasskey 添加到系统任务栏托盘。" },
		{ "Add", "添加" },
		{ "NoThanks", "不用了" },

		// 诊断页 (DiagnosticsPage)
		{ "IncompatibleVersion", "版本不兼容" },
		{ "VersionMismatch", "版本不匹配" },
		{ "ProviderRegistration", "提供程序注册" },
		{ "CredentialCache", "Windows 凭据缓存" },
		{ "SyncNow", "立即同步" },
		{ "SyncNowTooltip", "从当前已打开的 KeePass 数据库刷新 Windows 凭据缓存" },
		{ "WriteToLog", "写入日志" },
		{ "WriteToLogTooltip", "将 Windows 凭据缓存与已识别通行密钥记录到日志" },
		{ "Clear", "清空缓存" },
		{ "ClearTooltip", "清除 Windows 缓存中的所有通行密钥（下次打开或保存数据库时会自动重新填充）" },
		{ "CopyProviderVersion", "复制提供程序版本" },
		{ "CopyPluginVersion", "复制插件/服务版本" },
		{ "OpenLogDir", "打开日志目录" },
		{ "OpenLogFile", "打开日志文件" },
		{ "CopyLog", "复制日志到剪贴板" },

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
		{ "SystemTrayIcon", "系统托盘图标" },
		{ "SystemTrayIconDesc", "在任务栏托盘显示状态图标，关闭窗口后保持在托盘运行" },

		// 验证模式设置
		{ "HeaderVerification", "通知与用户验证" },
		{ "DescVerification", "通行密钥操作时的确认提示与用户身份验证方式" },
		{ "ApprovalTimeout", "审批等待超时" },
		{ "ApprovalTimeoutDesc", "等待用户确认操作的最长时间（秒）" },
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

		// 通行密钥条目设置
		{ "HeaderPasskeyEntries", "通行密钥条目" },
		{ "DescPasskeyEntries", "新建通行密钥条目时的命名与组织规则" },
		{ "SaveNewPasskeysIn", "新密钥存放分组" },
		{ "SaveNewPasskeysInDesc", "Passkeys 专属分组：专用分组。选中分组：创建时在 KeePass 中选中的当前分组。" },
		{ "EntryTitle", "新条目标题模板" },
		{ "EntryTitleDesc", "新条目的默认标题格式，支持 {RP_NAME} 及 {USERNAME}、{URL} 等占位符" },
		{ "StoreResolvedPlaceholders", "保存解析后的标题占位符" },
		{ "StoreResolvedPlaceholdersDesc", "开启：保存时直接解析并写入最终文本。关闭：每次展示条目时由 KeePass 动态解析。" },
		{ "TagNewPasskeys", "标记新通行密钥" },
		{ "TagNewPasskeysDesc", "为新建的通行密钥条目自动追加“Passkey”标签" },
		{ "AllowDuplicatePasskeys", "处理重复密钥策略" },
		{ "AllowDuplicatePasskeysDesc", "响应网站要求不重复添加第二张通行密钥时的检查范围" },

		// 高级设置
		{ "HeaderAdvanced", "高级设置" },
		{ "DescAdvanced", "运行行为、轮询计时与日志等低频修改项" },
		{ "LogLevel", "日志详细度" },
		{ "LogLevelDesc", "提供程序与插件日志的输出详细程度" },
		{ "StatusRefreshInterval", "状态刷新周期" },
		{ "StatusRefreshIntervalDesc", "应用程序轮询检查后台服务状态的时间间隔（秒）" },
		{ "OfferSavingToExistingEntry", "提供保存到已有条目选项" },
		{ "OfferSavingToExistingEntryDesc", "当已有条目匹配该网站与用户名时，在弹窗中允许直接保存到已有条目" },
		{ "CheckForPluginUpdates", "检查插件更新" },
		{ "CheckForPluginUpdatesDesc", "KeePass 启动时，若检测到更新的插件则提示升级" },
		{ "UseLegacyNotificationPrompts", "使用系统通知代替弹窗" },
		{ "UseLegacyNotificationPromptsDesc", "使用 Windows 气泡通知而非桌面交互对话框展示确认提示" },

		// 专家设置
		{ "HeaderExpert", "专家选项" },
		{ "DescExpert", "针对特殊环境的高级选项，通常保持默认即可" },
		{ "WindowsCredentialCache", "Windows 凭据缓存" },
		{ "WindowsCredentialCacheDesc", "让您的通行密钥出现在 Windows 系统登录提示与自动填充列表中" },
		{ "BackupEligibility", "允许备份同步 (BE)" },
		{ "BackupEligibilityDesc", "开启时告知网站该密钥允许备份与多设备同步；关闭时告知仅存于本机单设备" },
		{ "BackupState", "已备份状态 (BS)" },
		{ "BackupStateDesc", "告知网站该密钥当前已经完成备份（仅在开启允许备份时可用）" },
		{ "SpoofAaguid", "伪装认证器标识 (AAGUID)" },
		{ "SpoofAaguidDesc", "重写网站在创建密钥时看到的 AAGUID 厂商标识，留空使用默认值。应用后将自动向 Windows 重新注册。" },
		{ "ClearField", "清空输入框" },

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

		// 插件安装对话框
		{ "KeePassFolderCaption", "KeePass 安装目录或 Plugins 插件目录" },
		{ "InstallsTo", "将安装至" },
		{ "FolderCreatedNotice", "该文件夹尚不存在，安装时将自动创建。" },
		{ "ElevationNotice", "向该位置写入需要管理员权限，Windows 将弹出权限确认。" },
		{ "CopyFileYourself", "或者您也可以手动复制文件：" },
		{ "ShowTargetFolder", "打开目标文件夹" },
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
		{ "NotConnected", "Not connected" },
		{ "NotDetected", "Not detected" },
		{ "Registered", "Registered" },
		{ "NotRegistered", "Not registered" },
		{ "Ready", "Ready" },
		{ "Browse", "Browse…" },

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
		{ "ReplaceWarning", "This entry already has a passkey. Saving replaces it." },

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
		{ "OpenUserGuide", "Open user guide" },
		{ "Step1Title", "1. Install the plugin or start KeePassXC" },
		{ "Step1Desc", "Keep KeePassXC running with your database unlocked, or put the plugin in KeePass Plugins folder." },
		{ "InstallPlugin", "Install plugin" },
		{ "InstallPluginTooltip", "Install or update the KeePass plugin" },
		{ "ShowPluginFile", "Show plugin file" },
		{ "ShowPluginFileTooltip", "Reveal KeePassPasskey.dll in File Explorer" },
		{ "Step2Title", "2. Open Advanced Passkey Options" },
		{ "Step2Desc", "Go to Settings > Accounts > Passkeys > Advanced options, or use the button below." },
		{ "Step3Title", "3. Enable KeePassPasskey" },
		{ "Step3Desc", "Toggle KeePassPasskey on in the passkey provider list." },
		{ "Step4Title", "4. Verify & use" },
		{ "Step4Desc", "Both statuses above should read Enabled / Running. You can now use passkeys in your browser." },
		{ "TrayOfferTitle", "Want to keep an eye on the status?" },
		{ "TrayOfferDesc", "Add KeePassPasskey to the system tray." },
		{ "Add", "Add" },
		{ "NoThanks", "No thanks" },

		{ "IncompatibleVersion", "Incompatible version" },
		{ "VersionMismatch", "Version mismatch" },
		{ "ProviderRegistration", "Provider registration" },
		{ "CredentialCache", "Credential cache" },
		{ "SyncNow", "Sync now" },
		{ "SyncNowTooltip", "Refresh the Windows cache from the open KeePass databases now" },
		{ "WriteToLog", "Write to log" },
		{ "WriteToLogTooltip", "Write the Windows cache and the KeePass passkeys to the log" },
		{ "Clear", "Clear" },
		{ "ClearTooltip", "Remove every passkey from the Windows cache. They return on the next database open or save." },
		{ "CopyProviderVersion", "Copy provider version" },
		{ "CopyPluginVersion", "Copy plugin version" },
		{ "OpenLogDir", "Open log directory" },
		{ "OpenLogFile", "Open log file" },
		{ "CopyLog", "Copy log to clipboard" },

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
		{ "SystemTrayIcon", "System tray icon" },
		{ "SystemTrayIconDesc", "Show a status icon in the system tray. Closing the window keeps the app running in the tray." },

		{ "HeaderVerification", "Notifications & User Verification" },
		{ "DescVerification", "Confirmation prompts and user verification for passkey operations" },
		{ "ApprovalTimeout", "Approval timeout" },
		{ "ApprovalTimeoutDesc", "How long the confirmation prompt stays open" },
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

		{ "HeaderPasskeyEntries", "Passkey Entries" },
		{ "DescPasskeyEntries", "How new passkey entries are created" },
		{ "SaveNewPasskeysIn", "Save new passkeys in" },
		{ "SaveNewPasskeysInDesc", "Passkeys group: the default group for passkeys. Selected group: the group selected in KeePass when creating the passkey." },
		{ "EntryTitle", "Entry title" },
		{ "EntryTitleDesc", "Title for new entries. Supports {RP_NAME} and KeePass placeholders like {USERNAME} or {URL}." },
		{ "StoreResolvedPlaceholders", "Store resolved title placeholders" },
		{ "StoreResolvedPlaceholdersDesc", "On: placeholders are resolved and the final text is saved in the title. Off: KeePass resolves them each time the entry is shown." },
		{ "TagNewPasskeys", "Tag new passkeys" },
		{ "TagNewPasskeysDesc", "Add a 'Passkey' tag to each entry created when a new passkey is registered" },
		{ "AllowDuplicatePasskeys", "Allow duplicate passkeys" },
		{ "AllowDuplicatePasskeysDesc", "Where to enforce a website's request not to add a second passkey for an account it already has one for." },

		{ "HeaderAdvanced", "Advanced" },
		{ "DescAdvanced", "Behaviour, timing and logging settings that are rarely changed" },
		{ "LogLevel", "Log level" },
		{ "LogLevelDesc", "Verbosity of the provider and plugin log files" },
		{ "StatusRefreshInterval", "Status refresh interval" },
		{ "StatusRefreshIntervalDesc", "How often the app polls for provider status" },
		{ "OfferSavingToExistingEntry", "Offer saving to an existing entry" },
		{ "OfferSavingToExistingEntryDesc", "Offer to save a passkey onto a matching entry instead of always creating a new one." },
		{ "CheckForPluginUpdates", "Check for plugin updates" },
		{ "CheckForPluginUpdatesDesc", "When KeePass starts, offer to update the plugin if this app ships a newer one." },
		{ "UseLegacyNotificationPrompts", "Use legacy notification prompts" },
		{ "UseLegacyNotificationPromptsDesc", "Show confirmation prompts as Windows notifications instead of dialogs." },

		{ "HeaderExpert", "Expert" },
		{ "DescExpert", "Advanced options for uncommon setups. Change only if you know why." },
		{ "WindowsCredentialCache", "Windows credential cache" },
		{ "WindowsCredentialCacheDesc", "Lets your passkeys appear in the Windows sign-in prompt and autofill." },
		{ "BackupEligibility", "Backup Eligibility (BE)" },
		{ "BackupEligibilityDesc", "When on, websites are told that a new passkey is allowed to be backed up and synced to your other devices." },
		{ "BackupState", "Backup State (BS)" },
		{ "BackupStateDesc", "When on, websites are told that a new passkey is backed up right now." },
		{ "SpoofAaguid", "Spoof AAGUID" },
		{ "SpoofAaguidDesc", "Override the authenticator identifier (AAGUID) that websites see when you create a passkey." },
		{ "ClearField", "Clear field" },

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

		{ "KeePassFolderCaption", "KeePass folder or plugins folder" },
		{ "InstallsTo", "Installs to" },
		{ "FolderCreatedNotice", "This folder does not exist yet and will be created." },
		{ "ElevationNotice", "Writing here needs administrator rights. Windows asks for them once." },
		{ "CopyFileYourself", "Or copy the file yourself:" },
		{ "ShowTargetFolder", "Show target folder" },
	};
}
