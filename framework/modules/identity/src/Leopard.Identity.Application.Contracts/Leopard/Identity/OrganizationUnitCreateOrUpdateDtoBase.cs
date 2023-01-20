using System.ComponentModel.DataAnnotations;
using Volo.Abp.ObjectExtending;

namespace Leopard.Identity
{
	public abstract class OrganizationUnitCreateOrUpdateDtoBase : ExtensibleObject
	{
		/// <summary>
		/// 显示名称
		/// </summary>
		[Required]
		[StringLength(128)]
		public string DisplayName { get; set; }
	}
}
