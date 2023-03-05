using Leopard.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.OpenIddict.EntityFrameworkCore;

namespace EShop.Identity.EntityFrameworkCore;

[ConnectionStringName("Default")]
public class IdentityDbContext :
    LeopardDbContext<IdentityDbContext>
{
    /* Add DbSet properties for your Aggregate Roots / Entities here. */


    public IdentityDbContext(DbContextOptions<IdentityDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.UseCollation("utf8mb4_0900_as_cs");

        builder.ConfigureIdentity();
        builder.ConfigureOpenIddict();

        builder.LeopardDbMapper();
    }
}
