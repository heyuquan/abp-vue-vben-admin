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
    public class EfCoreEditionRepository : EfCoreRepository<ISaasDbContext, Edition, Guid>, IReadOnlyBasicRepository<Edition, Guid>, IReadOnlyBasicRepository<Edition>, IBasicRepository<Edition>, IBasicRepository<Edition, Guid>, IRepository, IEditionRepository
	{
		public EfCoreEditionRepository(IDbContextProvider<ISaasDbContext> dbContextProvider)
			: base(dbContextProvider)
		{
		}

		public virtual async Task<List<Edition>> GetListAsync(string sorting = null, int maxResultCount = 2147483647, int skipCount = 0, string filter = null, bool includeDetails = false, CancellationToken cancellationToken = default(CancellationToken))
		{
			bool condition = !filter.IsNullOrWhiteSpace();

			return await (await GetDbSetAsync()).WhereIf(condition, x => x.DisplayName.Contains(filter)).OrderBy(sorting ?? "DisplayName").PageBy(skipCount, maxResultCount).ToListAsync(GetCancellationToken(cancellationToken));
		}

		public virtual async Task<int> GetCountAsync(string filter, CancellationToken cancellationToken = default(CancellationToken))
		{
			bool condition = !filter.IsNullOrWhiteSpace();
			return await (await GetDbSetAsync()).WhereIf(condition, x => x.DisplayName.Contains(filter)).CountAsync(GetCancellationToken(cancellationToken));
		}

		public virtual async Task<bool> CheckNameExistAsync(string displayName, CancellationToken cancellationToken = default(CancellationToken))
		{
			return await (await GetDbSetAsync()).Where(x => x.DisplayName == displayName).AnyAsync(GetCancellationToken(cancellationToken));
		}
	}
}
