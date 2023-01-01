namespace Leopard.Identity
{
	/// <summary>
	/// 身份标识锁定设置
	/// </summary>
	public class IdentityLockoutSettingsDto
	{
		/// <summary>
		/// 允许新用户
		/// </summary>
		public bool AllowedForNewUsers { get; set; }
		/// <summary>
		/// 锁定时间（秒）
		/// </summary>
		public int LockoutDuration { get; set; }
		/// <summary>
		/// 最大失败访问尝试次数
		/// </summary>
		public int MaxFailedAccessAttempts { get; set; }
	}
}
