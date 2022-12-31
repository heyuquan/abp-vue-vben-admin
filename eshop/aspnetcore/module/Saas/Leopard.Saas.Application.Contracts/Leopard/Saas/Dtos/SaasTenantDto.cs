using System;
using Volo.Abp.Application.Dtos;

namespace Leopard.Saas.Dtos
{
    public class SaasTenantDto : ExtensibleEntityDto<Guid>
	{
		/// <summary>
		/// 租户名称
		/// </summary>
		public string Name { get; set; }
		/// <summary>
		/// 版本Id
		/// </summary>
		public Guid? EditionId { get; set; }
		/// <summary>
		/// 版本名称
		/// </summary>
		public string EditionName { get; set; }
	}
}
