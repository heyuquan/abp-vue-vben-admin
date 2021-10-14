using System;

namespace Leopard.Identity
{
	/// <summary>
	/// 身份标识角色声明
	/// </summary>
	public class IdentityRoleClaimDto
	{
		/// <summary>
		/// 角色
		/// </summary>
		public Guid RoleId { get; set; }
		/// <summary>
		/// 声明类型
		/// </summary>
		public string ClaimType { get; set; }
		/// <summary>
		/// 声明值
		/// </summary>
		public string ClaimValue { get; set; }
	}
}
