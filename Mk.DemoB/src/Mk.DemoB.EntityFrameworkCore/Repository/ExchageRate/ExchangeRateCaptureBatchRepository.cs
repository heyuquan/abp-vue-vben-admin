using Mk.DemoB.EntityFrameworkCore;
using Mk.DemoB.ExchangeRateMgr;
using Mk.DemoB.ExchangeRateMgr.Entities;
using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Mk.DemoB.Repository
{
    public class ExchangeRateCaptureBatchRepository : EfCoreRepository<DemoBDbContext, ExchangeRateCaptureBatch, Guid>, IExchangeRateCaptureBatchRepository
    {
        public ExchangeRateCaptureBatchRepository(IDbContextProvider<DemoBDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<ExchangeRateCaptureBatch> GetByCaptureBatchNumberAsync(string captureBatchNumber)
        {
            return await GetQueryable().Where(x => x.CaptureBatchNumber == captureBatchNumber).FirstOrDefaultAsync();
        }

        public async Task<ExchangeRateCaptureBatch> GetTopOneAsync()
        {
            return await GetQueryable().OrderByDescending(x => x.Id).FirstOrDefaultAsync();
        }

    }
}
