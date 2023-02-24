using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Microsoft.EntityFrameworkCore
{
    public static class ModelBuilderExtensions
    {
        public static void LeopardDbMapper(this ModelBuilder builder, LeopardModelBuilderOptions options = null)
        {
            if (options == null) options = new LeopardModelBuilderOptions();

            foreach (var entity in builder.Model.GetEntityTypes())
            {
                var properties = entity.GetProperties();

                if (options.EnableCameNamelToUnder)
                    CameNamelToUnder(builder, entity, properties);

                if (options.DefaultDecimalPrecisionOptions.IsEnable)
                    DefauleDecimalPrecision(builder, entity, properties, options.DefaultDecimalPrecisionOptions);
            }
        }

        /// <summary>
        /// 将大驼峰命名转为mysql规范的小写加下划线  Eg：AbpUsers >> abp_users
        /// </summary>
        private static void CameNamelToUnder(ModelBuilder builder, IMutableEntityType entity, IEnumerable<IMutableProperty> properties)
        {
            var currentTableName = builder.Entity(entity.Name).Metadata.GetTableName();

            builder.Entity(entity.Name).ToTable(currentTableName.CamelToUnder());

            foreach (var property in properties)
            {
                property.SetColumnName(property.Name.CamelToUnder());
                //builder.Entity(entity.Name).Property(property.Name).HasColumnName(property.Name.CamelToUnder());
            }
        }

        private static void DefauleDecimalPrecision(ModelBuilder builder
            , IMutableEntityType entity
            , IEnumerable<IMutableProperty> properties
            , DefaultDecimalPrecisionOptions options
            )
        {
            foreach (var property in properties)
            {
                if (Type.GetTypeCode(property.ClrType) == TypeCode.Decimal)
                {
                    var precis = property.PropertyInfo.GetCustomAttribute<DecimalPrecisionAttribute>();
                    if (precis == null)
                    {
                        property.SetPrecision(options.Precision);
                        property.SetScale(options.Scale);
                        //builder.Entity(entity.Name).Property(property.Name).HasPrecision(options.Precision, options.Scale);
                    }
                    else
                    {
                        property.SetPrecision(precis.Precision);
                        property.SetScale(precis.Scale);
                    }
                }
            }
        }
    }

}

