using JetBrains.Annotations;
using Leopard.EntityFrameworkCore.Logger;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionEFLoggerExtensions
    {
        public static void AddEFLogger([NotNull] this IServiceCollection services)
        {
            services.AddTransient(typeof(EFLogger));
            services.AddTransient(typeof(EFLoggerProvider));
        }
    }
}
