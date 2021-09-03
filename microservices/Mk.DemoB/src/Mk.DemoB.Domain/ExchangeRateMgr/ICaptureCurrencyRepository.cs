using Mk.DemoB.ExchangeRateMgr.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Repositories;

namespace Mk.DemoB.ExchangeRateMgr
{
    public interface ICaptureCurrencyRepository : IBasicRepository<CaptureCurrency, Guid>
    {
    }
}
