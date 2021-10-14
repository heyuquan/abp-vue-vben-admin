using Volo.Abp.Application.Dtos;

namespace Leopard.Identity
{
	public class GetIdentityClaimTypesInput : PagedAndSortedResultRequestDto
	{
		public string Filter { get; set; }

	}
}
