using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Leopard.Identity
{
	public class OrganizationUnitWithDetailsDto : ExtensibleFullAuditedEntityDto<Guid>
	{
		/// <summary>
		/// 上级节点Id
		/// </summary>
		public Guid? ParentId { get; set; }

		public string Code { get; set; }
		/// <summary>
		/// 显示名称
		/// </summary>
		public string DisplayName { get; set; }
		/// <summary>
		/// 组织的角色集合
		/// </summary>
		public List<IdentityRoleDto> Roles { get; set; }
	}
}
