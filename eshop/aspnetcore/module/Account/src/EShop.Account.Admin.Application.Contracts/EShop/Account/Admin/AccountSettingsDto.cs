namespace EShop.Account.Admin
{
	/// <summary>
	/// 账户设置
	/// </summary>
	public class AccountSettingsDto
	{
		/// <summary>
		/// 是否启用自我注册
		/// </summary>
		public bool IsSelfRegistrationEnabled { get; set; }
		/// <summary>
		///使用本地账户进行身份验证
		/// </summary>
		public bool EnableLocalLogin { get; set; }
		/// <summary>
		///记住这个浏览器
		/// </summary>
		public bool IsRememberBrowserEnabled { get; set; }
	}
}
