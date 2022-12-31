using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Leopard.Saas.EntityFrameworkCore
{
    [ConnectionStringName(SaasServiceDbProperties.ConnectionStringName)]
	public interface ISaasDbContext : IEfCoreDbContext
	{
		DbSet<Tenant> Tenants { get; }

		DbSet<Edition> Editions { get; }

		DbSet<TenantConnectionString> TenantConnectionStrings { get; }
	}
}

