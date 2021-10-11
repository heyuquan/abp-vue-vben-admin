using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Leopard.Saas
{
	public interface IEditionRepository : IBasicRepository<Edition>, IReadOnlyBasicRepository<Edition, Guid>, IReadOnlyBasicRepository<Edition>, IBasicRepository<Edition, Guid>, IRepository
	{
		Task<List<Edition>> GetListAsync(string sorting = null, int maxResultCount = 2147483647, int skipCount = 0, string filter = null, bool includeDetails = false, CancellationToken cancellationToken = default(CancellationToken));

		Task<int> GetCountAsync(string filter, CancellationToken cancellationToken = default(CancellationToken));

		Task<bool> CheckNameExistAsync(string displayName, CancellationToken cancellationToken = default(CancellationToken));
	}
}
