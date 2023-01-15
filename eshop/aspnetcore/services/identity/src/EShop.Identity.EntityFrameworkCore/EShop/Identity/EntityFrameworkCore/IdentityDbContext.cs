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

        /* Include modules to your migration db context */

        //builder.ConfigurePermissionManagement();
        builder.ConfigureIdentity();
        builder.ConfigureOpenIddict();

        /* Configure your own tables/entities inside here */

        //builder.Entity<YourEntity>(b =>
        //{
        //    b.ToTable(IdentityConsts.DbTablePrefix + "YourEntities", IdentityConsts.DbSchema);
        //    b.ConfigureByConvention(); //auto configure for the base class props
        //    //...
        //});
    }
}
