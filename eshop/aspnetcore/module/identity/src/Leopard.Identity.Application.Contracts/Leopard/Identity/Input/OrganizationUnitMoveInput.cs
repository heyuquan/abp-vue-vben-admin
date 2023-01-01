using System;

namespace Leopard.Identity
{
	/// <summary>
	/// 修改组织上级节点
	/// </summary>
	public class OrganizationUnitMoveInput
	{
		/// <summary>
		/// 新上节点Id
		/// </summary>
		public Guid? NewParentId { get; set; }
	}
}
