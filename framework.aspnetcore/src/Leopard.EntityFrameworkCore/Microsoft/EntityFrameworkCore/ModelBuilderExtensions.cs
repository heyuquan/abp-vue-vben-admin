using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.EntityFrameworkCore
{
    public static class ModelBuilderExtensions
    {
        public static void DbMapperCameNamelToUnder(this ModelBuilder builder)
        {
            // 将大驼峰命名转为mysql规范的小写加下划线  Eg：AbpUsers >> abp_users
            foreach (var entity in builder.Model.GetEntityTypes())
            {
                var currentTableName = builder.Entity(entity.Name).Metadata.GetTableName();

                builder.Entity(entity.Name).ToTable(currentTableName.CamelToUnder());

                var properties = entity.GetProperties();
                foreach (var property in properties)
                    builder.Entity(entity.Name).Property(property.Name).HasColumnName(property.Name.CamelToUnder());
            }
        }
    }

}
