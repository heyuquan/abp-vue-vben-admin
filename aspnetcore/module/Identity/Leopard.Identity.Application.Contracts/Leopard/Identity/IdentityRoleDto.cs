using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace Leopard.Identity
{
	/// <summary>
	/// 身份标识角色信息
	/// </summary>
	public class IdentityRoleDto : ExtensibleEntityDto<Guid>, IHasConcurrencyStamp
	{
		/// <summary>
		/// 角色名称
		/// </summary>
		public string Name { get; set; }
		/// <summary>
		/// 是否默认
		/// </summary>
		public bool IsDefault { get; set; }
		/// <summary>
		/// 是否静态
		/// </summary>
		public bool IsStatic { get; set; }
		/// <summary>
		/// 是否公开
		/// </summary>
		public bool IsPublic { get; set; }
		/// <summary>
		/// 时间戳
		/// </summary>
		public string ConcurrencyStamp { get; set; }
	}
}
