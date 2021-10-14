using System;

namespace Leopard.Identity
{
	/// <summary>
	/// 身份标识用户声明Dto
	/// </summary>
	public class IdentityUserClaimDto
	{
		/// <summary>
		/// 用户Id
		/// </summary>
		public Guid UserId { get; set; }
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
