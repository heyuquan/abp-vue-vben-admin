using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Leopard.Saas
{
    public interface ITenantRepository : IReadOnlyBasicRepository<Tenant, Guid>, IReadOnlyBasicRepository<Tenant>, IBasicRepository<Tenant>, IBasicRepository<Tenant, Guid>, IRepository
	{
		Task<Tenant> FindByNameAsync(string name, bool includeDetails = true, CancellationToken cancellationToken = default(CancellationToken));

		Task<List<Tenant>> GetListAsync(string sorting = null, int maxResultCount = 2147483647, int skipCount = 0, string filter = null, bool includeDetails = false, CancellationToken cancellationToken = default(CancellationToken));

		Tenant FindByName(string name, bool includeDetails = true);

		Tenant FindById(Guid id, bool includeDetails = true);

		Task<long> GetCountAsync(string filter = null, CancellationToken cancellationToken = default(CancellationToken));
	}
}
