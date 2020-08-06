using System;
using Leopard.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.SettingManagement;
using Volo.Abp.SettingManagement.EntityFrameworkCore;

namespace Leopard.Abp.SettingManagement.EntityFrameworkCore
{
    [ConnectionStringName(LeopardDbProperties.DefaultDbConnectionStringName)]
    public class LeopardSettingManagementDbContext : AbpDbContext<LeopardSettingManagementDbContext>, ISettingManagementDbContext
    {
        public DbSet<Setting> Settings { get; set; }

        public LeopardSettingManagementDbContext(DbContextOptions<LeopardSettingManagementDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureSettingManagement();

            builder.DbMapperCameNamelToUnder();
        }
    }
}
