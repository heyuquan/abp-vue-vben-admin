using System;
using Volo.Abp.Application.Dtos;

namespace Leopard.Identity
{
	public class OrganizationUnitRoleDto : CreationAuditedEntityDto
	{
		/// <summary>
		/// 组织Id
		/// </summary>
		public Guid OrganizationUnitId { get; set; }
		/// <summary>
		/// 角色Id
		/// </summary>
		public Guid RoleId { get; set; }
	}
}
