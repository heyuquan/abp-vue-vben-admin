using Mk.DemoB.EntityFrameworkCore;
using Mk.DemoB.ExchangeRateMgr;
using Mk.DemoB.ExchangeRateMgr.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Mk.DemoB.Repository.ExchageRate
{
    public class CaptureCurrencyRepository : EfCoreRepository<DemoBDbContext, CaptureCurrency, Guid>, ICaptureCurrencyRepository
    {
        public CaptureCurrencyRepository(IDbContextProvider<DemoBDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}
