using System.ComponentModel.DataAnnotations;
using Volo.Abp.Identity;
using Volo.Abp.Validation;

namespace Leopard.Identity
{
    public class IdentityRoleClaimUpdateInput : IdentityRoleClaimCreateInput
    {
        [Required]
        [DynamicMaxLength(typeof(IdentityRoleClaimConsts), nameof(IdentityRoleClaimConsts.MaxClaimValueLength))]
        public string NewClaimValue { get; set; }
    }
}
