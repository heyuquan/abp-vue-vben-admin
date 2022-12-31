using Volo.Abp.Domain.Entities;

namespace Leopard.Identity
{
	/// <summary>
	/// 身份标识用户更新
	/// </summary>
	public class IdentityUserUpdateDto : IdentityUserCreateOrUpdateDtoBase, IHasConcurrencyStamp
	{
		public string ConcurrencyStamp { get; set; }
	}
}
