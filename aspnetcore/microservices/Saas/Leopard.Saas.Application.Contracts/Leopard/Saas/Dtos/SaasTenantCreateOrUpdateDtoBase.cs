using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.ObjectExtending;

namespace Leopard.Saas.Dtos
{
    public abstract class SaasTenantCreateOrUpdateDtoBase : ExtensibleObject
	{
		/// <summary>
		/// 租户名称
		/// </summary>
		[StringLength(64)]
		[Required]
		public string Name { get; set; }
		/// <summary>
		/// 版本Id
		/// </summary>
		public Guid? EditionId { get; set; }

		protected SaasTenantCreateOrUpdateDtoBase()
			: base(false)
		{
		}
	}
}
