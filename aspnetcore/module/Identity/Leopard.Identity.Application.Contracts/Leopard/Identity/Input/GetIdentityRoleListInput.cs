using Volo.Abp.Application.Dtos;

namespace Leopard.Identity
{
	public class GetIdentityRoleListInput : PagedAndSortedResultRequestDto
	{
		public string Filter { get; set; }
	}
}
