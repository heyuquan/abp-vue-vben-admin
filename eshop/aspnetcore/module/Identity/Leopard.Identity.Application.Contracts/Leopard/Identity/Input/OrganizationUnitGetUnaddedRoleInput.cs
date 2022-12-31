using Volo.Abp.Application.Dtos;

namespace Leopard.Identity
{
    public class OrganizationUnitGetUnaddedRoleInput : PagedAndSortedResultRequestDto
    {

        public string Filter { get; set; }
    }
}
