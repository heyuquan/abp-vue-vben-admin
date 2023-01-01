using System;

namespace Leopard.Identity
{
	public class OrganizationUnitCreateDto : OrganizationUnitCreateOrUpdateDtoBase
	{
		/// <summary>
		/// 上级节点Id
		/// </summary>
		public Guid? ParentId { get; set; }
	}
}
