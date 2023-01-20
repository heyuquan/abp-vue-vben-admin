using System.ComponentModel.DataAnnotations;

namespace Leopard.Identity
{
	/// <summary>
	/// 身份标识用户更新角色信息
	/// </summary>
	public class IdentityUserUpdateRolesDto
	{
		/// <summary>
		/// 角色名集合
		/// </summary>
		[Required]
		public string[] RoleNames { get; set; }
	}
}
