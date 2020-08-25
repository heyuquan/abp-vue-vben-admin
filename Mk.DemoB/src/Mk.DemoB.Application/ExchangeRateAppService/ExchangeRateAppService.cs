using Leopard.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;

namespace Mk.DemoB.ExchangeRateAppService
{
    /// <summary>
    /// 汇率
    /// </summary>
    [Route("api/demob/exchange-rate")]
    public class ExchangeRateAppService : DemoBAppService
    {
        private readonly ExchangeRateManager _exchangeRateManager;
        private readonly IRepository<CaptureCurrency, Guid> _captureCurrencyRepository;
        private readonly IRepository<ExchangeRateCaptureBatch, Guid> _exchangeRateCaptureBatchRepository;
        private readonly IRepository<ExchangeRate, Guid> _exchangeRateRepository;

        public ExchangeRateAppService(
            IHttpClientFactory clientFactory
            , ExchangeRateManager exchangeRateManager
            , IRepository<ExchangeRate, Guid> exchangeRateRepository
            , IRepository<CaptureCurrency, Guid> captureCurrencyRepository
            , IRepository<ExchangeRateCaptureBatch, Guid> exchangeRateCaptureBatchRepository
            )
        {
            _exchangeRateManager = exchangeRateManager;
            _captureCurrencyRepository = captureCurrencyRepository;
            _exchangeRateCaptureBatchRepository = exchangeRateCaptureBatchRepository;
            _exchangeRateRepository = exchangeRateRepository;
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
        /// 获取批次的汇率数据
        /// </summary>
        /// <param name="captureBatchNumber">可空，若为空，则获取最新批次的汇率数据</param>
        /// <returns></returns>
        [HttpGet("batch")]
        public async Task<ServiceResult<ExchangeRateBatchDto>> GetLateastBatchRate(string captureBatchNumber)
        {
            ServiceResult<ExchangeRateBatchDto> ret = new ServiceResult<ExchangeRateBatchDto>(IdProvider.Get());
            ExchangeRateBatchDto retDto = new ExchangeRateBatchDto();
            ExchangeRateCaptureBatch batch = null;
            if (string.IsNullOrWhiteSpace(captureBatchNumber))
            {
                batch = await _exchangeRateCaptureBatchRepository.OrderByDescending(x => x.Id).FirstOrDefaultAsync();
            }
            else
            {
                batch = await _exchangeRateCaptureBatchRepository.Where(x => x.CaptureBatchNumber == captureBatchNumber).FirstOrDefaultAsync();
            }

            if (batch != null)
            {
                var exchangeRates = await _exchangeRateRepository.Where(x => x.CaptureBatchNumber == batch.CaptureBatchNumber).ToListAsync();
                retDto.CaptureTime = batch.CaptureTime;
                retDto.CaptureBatchNumber = batch.CaptureBatchNumber;
                List<ExchangeRateDto> exchangeRateDtos = ObjectMapper.Map<List<ExchangeRate>, List<ExchangeRateDto>>(exchangeRates);
                retDto.ExchangeRates = exchangeRateDtos;
                ret.SetSuccess(retDto);
            }
            else
            {
                UserFriendlyException exception = new UserFriendlyException(
                    message: "没有任何汇率数据"
                   , logLevel: LogLevel.Error
                   );
                throw exception;
            }

            return ret;
        }

        /// <summary>
        /// 获取汇率数据的Top条记录
        /// </summary>
        /// <param name="currencyCodeFrom">来源币种（eg：CNY）</param>
        /// <param name="currencyCodeTo">目的币种（eg：USD）</param>
        /// <param name="topCount">按时间排序，返回多少条数据。最新时间的数据为第一条</param>
        /// <returns></returns>
        [HttpGet("top-record")]
        public async Task<ServiceResult<List<ExchangeRateDto>>> GetTopRateAsync(string currencyCodeFrom, string currencyCodeTo, int topCount = 1)
        {
            ServiceResult<List<ExchangeRateDto>> ret = new ServiceResult<List<ExchangeRateDto>>(IdProvider.Get());

            var exchangeRates = await _exchangeRateManager.GetTopRateAsync(currencyCodeFrom, currencyCodeTo, topCount);
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

        /// <summary>
        /// 获取汇率分页数据
        /// </summary>
        /// <returns></returns>
        [HttpGet("paging")]
        public async Task<ServiceResult<PagedResultDto<ExchangeRateDto>>> GetRatePagingAsync(GetRatePagingRequest req)
        {
            ServiceResult<PagedResultDto<ExchangeRateDto>> ret = new ServiceResult<PagedResultDto<ExchangeRateDto>>(IdProvider.Get());

            var exchangeRates = await _exchangeRateManager.GetRatePagingAsync(req.CurrencyCodeFrom, req.CurrencyCodeTo, req.BeginTime, req.EndTime, req.SkipCount, req.MaxResultCount);
            var count = await _exchangeRateManager.GetRatePagingCountAsync(req.CurrencyCodeFrom, req.CurrencyCodeTo, req.BeginTime, req.EndTime);
            if (exchangeRates.Any())
            {
                List<ExchangeRateDto> dtos = ObjectMapper.Map<List<ExchangeRate>, List<ExchangeRateDto>>(exchangeRates);
                var pagDto = new PagedResultDto<ExchangeRateDto>(count, dtos);
                ret.SetSuccess(pagDto);
            }
            else
            {
                throw new UserFriendlyException(message: "传入的币种，获取不到汇率数据", logLevel: LogLevel.Error);
            }

            return ret;
        }
    }
}
