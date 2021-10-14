using Volo.Abp.Application.Dtos;

namespace Leopard.Identity
{
	public class GetIdentityUsersInput : PagedAndSortedResultRequestDto
	{
		public string Filter { get; set; }
	}
}
