namespace Leopard.Identity
{
	/// <summary>
	/// 身份标识登录设置 
	/// </summary>
	public class IdentitySignInSettingsDto
	{
		/// <summary>
		/// 登录时是否需要验证的电子邮箱.
		/// </summary>
		public bool RequireConfirmedEmail { get; set; }
		/// <summary>
		/// 用户是否可以确认电话号码
		/// </summary>
		public bool EnablePhoneNumberConfirmation { get; set; }
		/// <summary>
		/// 登录时是否需要验证的手机号码.
		/// </summary>
		public bool RequireConfirmedPhoneNumber { get; set; }
	}
}
