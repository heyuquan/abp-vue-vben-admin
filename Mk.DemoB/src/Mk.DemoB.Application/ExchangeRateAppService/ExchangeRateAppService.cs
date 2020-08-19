using HtmlAgilityPack;
using Leopard.Result;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Mk.DemoB.Dto.ExchangeRates;
using Mk.DemoB.ExchangeRateMgr;
using Mk.DemoB.ExchangeRateMgr.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;

namespace Mk.DemoB.ExchangeRateAppService
{
    /// <summary>
    /// 汇率
    /// </summary>
    [Route("exchange-rate")]
    public class ExchangeRateAppService : DemoBAppService
    {
        private readonly ExchangeRateManager _exchangeRateManager;
        private readonly IRepository<CaptureCurrency, Guid> _captureCurrencyRepository;

        public ExchangeRateAppService(
            IHttpClientFactory clientFactory
            , ExchangeRateManager exchangeRateManager
            , IRepository<CaptureCurrency, Guid> captureCurrencyRepository
            )
        {
            _exchangeRateManager = exchangeRateManager;
            _captureCurrencyRepository = captureCurrencyRepository;
        }

        /// <summary>
        /// 创建一笔币别对应关系
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost("capture-currency/create")]
        public async Task<ServiceResult<CaptureCurrencyDto>> AddCaptureCurrencyAsync(AddCaptureCurrencyRequest req)
        {
            ServiceResult<CaptureCurrencyDto> ret = new ServiceResult<CaptureCurrencyDto>(IdProvider.Get());

            CaptureCurrency entity = new CaptureCurrency(GuidGenerator.Create(), req.CurrencyCodeFrom, req.CurrencyCodeTo);
            await _captureCurrencyRepository.InsertAsync(entity);

            CaptureCurrencyDto dto = ObjectMapper.Map<CaptureCurrency, CaptureCurrencyDto>(entity);
            ret.SetSuccess(dto);
            return ret;
        }

        /// <summary>
        /// 抓取所有CaptureCurrency表中指定的汇率
        /// </summary>
        [HttpPost("capture/all")]
        public async Task<ServiceResult<List<ExchangeRateDto>>> CaptureAllRateAndSaveAsync()
        {
            ServiceResult<List<ExchangeRateDto>> ret = new ServiceResult<List<ExchangeRateDto>>(IdProvider.Get());

            var exchangeRates = await _exchangeRateManager.CaptureAllRateAndSaveAsync();
            List<ExchangeRateDto> dtos = ObjectMapper.Map<List<ExchangeRate>, List<ExchangeRateDto>>(exchangeRates);
            ret.SetSuccess(dtos);
            return ret;
        }

        /// <summary>
        /// 获取最新批次的汇率数据
        /// </summary>
        /// <returns></returns>
        [HttpGet("lateast-batch")]
        public async Task<ServiceResult<List<ExchangeRateDto>>> GetLateastBatchRate()
        {
            ServiceResult<List<ExchangeRateDto>> ret = new ServiceResult<List<ExchangeRateDto>>(IdProvider.Get());

            var exchangeRates = await _exchangeRateManager.GetLateastBatchRate();
            List<ExchangeRateDto> dtos = ObjectMapper.Map<List<ExchangeRate>, List<ExchangeRateDto>>(exchangeRates);

            ret.SetSuccess(dtos);
            return ret;
        }

        /// <summary>
        /// 获取汇率
        /// </summary>
        /// <param name="currencyCodeFrom">来源币种（eg：CNY）</param>
        /// <param name="currencyCodeTo">目的币种（eg：USD）</param>
        /// <param name="topCount">按时间排序，返回多少条数据。最新时间的数据为第一条</param>
        /// <returns></returns>
        [HttpGet("one")]
        public async Task<ServiceResult<List<ExchangeRateDto>>> GetRateAsync(string currencyCodeFrom, string currencyCodeTo, int topCount = 1)
        {
            ServiceResult<List<ExchangeRateDto>> ret = new ServiceResult<List<ExchangeRateDto>>(IdProvider.Get());

            var exchangeRates = await _exchangeRateManager.GetRateAsync(currencyCodeFrom, currencyCodeTo, topCount);
            if (exchangeRates.Any())
            {
                List<ExchangeRateDto> dtos = ObjectMapper.Map<List<ExchangeRate>, List<ExchangeRateDto>>(exchangeRates);
                ret.SetSuccess(dtos);
            }
            else
            {
                UserFriendlyException exception = new UserFriendlyException(
                    message: "传入的币种，获取不到汇率数据"
                   , logLevel: LogLevel.Error
                   );
                throw exception;
            }

            return ret;
        }
    }
}
