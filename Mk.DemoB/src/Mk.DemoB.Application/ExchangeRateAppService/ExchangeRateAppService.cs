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
        private readonly ICaptureCurrencyRepository _captureCurrencyRepository;
        private readonly IExchangeRateCaptureBatchRepository _exchangeRateCaptureBatchRepository;
        private readonly IExchangeRateRepository _exchangeRateRepository;

        public ExchangeRateAppService(
            ExchangeRateManager exchangeRateManager
            , IExchangeRateRepository exchangeRateRepository
            , ICaptureCurrencyRepository captureCurrencyRepository
            , IExchangeRateCaptureBatchRepository exchangeRateCaptureBatchRepository
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
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("capture-currency/create")]
        public virtual async Task<ServiceResult<CaptureCurrencyDto>> CreateCaptureCurrencyAsync(CaptureCurrencyCreateRequest input)
        {
            ServiceResult<CaptureCurrencyDto> ret = new ServiceResult<CaptureCurrencyDto>(IdProvider.Get());

            CaptureCurrency entity = new CaptureCurrency(GuidGenerator.Create(), input.CurrencyCodeFrom, input.CurrencyCodeTo);
            await _captureCurrencyRepository.InsertAsync(entity);

            CaptureCurrencyDto dto = ObjectMapper.Map<CaptureCurrency, CaptureCurrencyDto>(entity);
            ret.SetSuccess(dto);
            return ret;
        }

        /// <summary>
        /// 抓取所有CaptureCurrency表中指定的汇率
        /// </summary>
        [HttpPost("capture/all")]
        public virtual async Task<ServiceResult<List<ExchangeRateDto>>> CaptureAllRateAndSaveAsync()
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
        /// <param name="captureBatchNumber">可空，若为空，则获取最新批次的汇率数据</param>
        /// <returns></returns>
        [HttpGet("batch/latest")]
        public virtual async Task<ServiceResult<ExchangeRateBatchDto>> GetLatestBatchPagingAsync()
        {
            ServiceResult<ExchangeRateBatchDto> ret = new ServiceResult<ExchangeRateBatchDto>(IdProvider.Get());
            ExchangeRateBatchDto retDto = new ExchangeRateBatchDto();
            ExchangeRateCaptureBatch batch = await _exchangeRateCaptureBatchRepository.GetTopOneAsync();

            if (batch != null)
            {
                var pageData = await _exchangeRateRepository.GetPagingAsync(
                      captureBatchNumber: batch.CaptureBatchNumber
                     , skipCount: 0
                     , maxResultCount: 1
                     , isGetTotalCount: false
                    );
                retDto.CaptureTime = batch.CaptureTime;
                retDto.CaptureBatchNumber = batch.CaptureBatchNumber;
                List<ExchangeRateDto> exchangeRateDtos = ObjectMapper.Map<List<ExchangeRate>, List<ExchangeRateDto>>(pageData.Items);
                retDto.ExchangeRates = exchangeRateDtos;
            }
            else
            {
                retDto.ExchangeRates = new List<ExchangeRateDto>();
            }

            ret.SetSuccess(retDto);
            return ret;
        }

        /// <summary>
        /// 获取汇率分页数据
        /// </summary>
        /// <returns></returns>
        [HttpGet("paging")]
        public virtual async Task<ServiceResult<PagedResultDto<ExchangeRateDto>>> GetExchangeRatePagingRequest(GetExchangeRatePagingRequest req)
        {
            ServiceResult<PagedResultDto<ExchangeRateDto>> ret = new ServiceResult<PagedResultDto<ExchangeRateDto>>(IdProvider.Get());

            var pageData = await _exchangeRateRepository.GetPagingAsync(
                                        currencyCodeFrom: req.CurrencyCodeFrom
                                        , currencyCodeTo: req.CurrencyCodeTo
                                        , beginTime: req.BeginTime
                                        , endTime: req.EndTime
                                        , sorting: req.Sorting
                                        , skipCount: req.SkipCount
                                        , maxResultCount: req.MaxResultCount);

            List<ExchangeRateDto> dtos = ObjectMapper.Map<List<ExchangeRate>, List<ExchangeRateDto>>(pageData.Items);
            var pagDto = new PagedResultDto<ExchangeRateDto>(pageData.TotalCount, dtos);
            ret.SetSuccess(pagDto);

            return ret;
        }
    }
}
