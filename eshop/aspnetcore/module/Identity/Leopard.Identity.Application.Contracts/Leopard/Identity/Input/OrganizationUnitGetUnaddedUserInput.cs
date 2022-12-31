using Volo.Abp.Application.Dtos;

namespace Leopard.Identity
{
    public class OrganizationUnitGetUnaddedUserInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}
