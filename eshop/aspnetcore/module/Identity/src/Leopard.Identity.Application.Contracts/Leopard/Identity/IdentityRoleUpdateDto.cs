using Volo.Abp.Domain.Entities;

namespace Leopard.Identity
{
	/// <summary>
	/// 身份标识角色更新
	/// </summary>
	public class IdentityRoleUpdateDto : IdentityRoleCreateOrUpdateDtoBase, IHasConcurrencyStamp
	{
		public string ConcurrencyStamp { get; set; }
	}
}
