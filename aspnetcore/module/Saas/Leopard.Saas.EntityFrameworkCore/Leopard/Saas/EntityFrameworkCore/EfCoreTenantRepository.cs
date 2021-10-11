using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Leopard.Saas.EntityFrameworkCore
{
    public class EfCoreTenantRepository : EfCoreRepository<ISaasDbContext, Tenant, Guid>, IReadOnlyBasicRepository<Tenant, Guid>, IReadOnlyBasicRepository<Tenant>, IBasicRepository<Tenant>, IBasicRepository<Tenant, Guid>, IRepository, ITenantRepository
	{
		public EfCoreTenantRepository(IDbContextProvider<ISaasDbContext> dbContextProvider)
			: base(dbContextProvider)
		{
		}

		public virtual async Task<Tenant> FindByNameAsync(string name, bool includeDetails = true, CancellationToken cancellationToken = default(CancellationToken))
		{
			return await (await GetDbSetAsync()).IncludeDetails(includeDetails).FirstOrDefaultAsync(x => x.Name == name, GetCancellationToken(cancellationToken));
		}

		public Tenant FindByName(string name, bool includeDetails = true)
		{
			return DbSet.IncludeDetails(includeDetails).FirstOrDefault(x => x.Name == name);
		}

		public Tenant FindById(Guid id, bool includeDetails = true)
		{
			return DbSet.IncludeDetails(includeDetails).FirstOrDefault(x => x.Id == id);
		}

		public virtual async Task<List<Tenant>> GetListAsync(string sorting = null, int maxResultCount = 2147483647, int skipCount = 0, string filter = null, bool includeDetails = false, CancellationToken cancellationToken = default(CancellationToken))
		{
			bool condition = !filter.IsNullOrWhiteSpace();
			return await (await GetDbSetAsync()).IncludeDetails(includeDetails).WhereIf(condition, x => x.Name.Contains(filter)).OrderBy(sorting ?? "Name").PageBy(skipCount, maxResultCount).ToListAsync(GetCancellationToken(cancellationToken));
		}

		public virtual async Task<long> GetCountAsync(string filter = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			bool condition = !filter.IsNullOrWhiteSpace();
			return await this.WhereIf(condition, x => x.Name.Contains(filter)).LongCountAsync(cancellationToken);
		}

		public override async Task<IQueryable<Tenant>> WithDetailsAsync()
		{
			return (await this.GetQueryableAsync()).IncludeDetails(true);
		}
	}
}
