using Volo.Abp.Application.Dtos;

namespace Leopard.Identity
{
	public class GetOrganizationUnitInput : PagedAndSortedResultRequestDto
	{
		public string Filter { get; set; }
	}
}
