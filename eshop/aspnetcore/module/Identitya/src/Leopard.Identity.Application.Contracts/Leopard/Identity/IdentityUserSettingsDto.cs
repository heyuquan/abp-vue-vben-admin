namespace Leopard.Identity
{
	/// <summary>
	/// 身份标识用户设置
	/// </summary>
	public class IdentityUserSettingsDto
	{
		/// <summary>
		/// 是否允许用户更新用户名.
		/// </summary>
		public bool IsUserNameUpdateEnabled { get; set; }
		/// <summary>
		/// 是否允许用户更新电子邮箱.
		/// </summary>
		public bool IsEmailUpdateEnabled { get; set; }
	}
}
