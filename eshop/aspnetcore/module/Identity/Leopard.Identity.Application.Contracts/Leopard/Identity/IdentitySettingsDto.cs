namespace Leopard.Identity
{
	/// <summary>
	/// 身份标识信息设置Dto
	/// </summary>
	public class IdentitySettingsDto
	{
		/// <summary>
		/// 密码设置
		/// </summary>
		public IdentityPasswordSettingsDto Password { get; set; }
		/// <summary>
		/// 账户锁定设置
		/// </summary>
		public IdentityLockoutSettingsDto Lockout { get; set; }
		/// <summary>
		/// 账户登录设置
		/// </summary>
		public IdentitySignInSettingsDto SignIn { get; set; }
		/// <summary>
		/// 用户设置
		/// </summary>
		public IdentityUserSettingsDto User { get; set; }
	}
}
