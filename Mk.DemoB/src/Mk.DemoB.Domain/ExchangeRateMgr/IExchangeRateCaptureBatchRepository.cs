using Mk.DemoB.ExchangeRateMgr.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Mk.DemoB.ExchangeRateMgr
{
    public interface IExchangeRateCaptureBatchRepository : IBasicRepository<ExchangeRateCaptureBatch, Guid>
    {
        Task<ExchangeRateCaptureBatch> GetByCaptureBatchNumberAsync(string captureBatchNumber);

        Task<ExchangeRateCaptureBatch> GetTopOneAsync();
    }
}
