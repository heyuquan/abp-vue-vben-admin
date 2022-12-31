using Volo.Abp.Auditing;

namespace Leopard.Identity
{
	/// <summary>
	/// 修改密码Input
	/// </summary>
	public class ChangePasswordInput
	{
		/// <summary>
		/// 当前密码
		/// </summary>
		[DisableAuditing]
		public string CurrentPassword { get; set; }

		/// <summary>
		/// 新密码
		/// </summary>
		[DisableAuditing]
		public string NewPassword { get; set; }
	}
}
