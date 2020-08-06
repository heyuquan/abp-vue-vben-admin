using Leopard.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.FeatureManagement;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;

namespace Leopard.Abp.FeatureManagement.EntityFrameworkCore
{
    [ConnectionStringName(LeopardDbProperties.DefaultDbConnectionStringName)]
    public class LeopardFeatureManagementDbContext : AbpDbContext<LeopardFeatureManagementDbContext>, IFeatureManagementDbContext
    {
        public DbSet<FeatureValue> FeatureValues { get; set; }

        public LeopardFeatureManagementDbContext(DbContextOptions<LeopardFeatureManagementDbContext> options) 
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureFeatureManagement();

            builder.DbMapperCameNamelToUnder();
        }
    }
}