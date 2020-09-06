using Leopard.Results;
using Microsoft.AspNetCore.Mvc;
using Mk.DemoB.Dto.SaleOrders;
using Mk.DemoB.IAppService;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Json;
using Volo.Abp.Serialization;

namespace Mk.DemoB.SerializerAppService
{
    // 程序集：Volo.Abp.Json，接口：IJsonSerializer  abp对接了Newtonsoft
    // 程序集：Volo.Abp.Serialization， 接口：IObjectSerializer  二进制序列化  可以使用IObjectSerializer<T>来做定制化序列化
    // 二进制序列化，对象需要标注[Serializable]
    [Route("api/demob/serializer")]
    public class SerializerAppService : DemoBAppService
    {
        private readonly IJsonSerializer _jsonSerializer;
        private readonly IObjectSerializer _objectSerializer;
        private readonly ISaleOrderAppService _saleOrderAppService;


        public SerializerAppService(
            IJsonSerializer jsonSerializer
            , IObjectSerializer objectSerializer
            , ISaleOrderAppService saleOrderAppService
            )
        {
            _jsonSerializer = jsonSerializer;
            _objectSerializer = objectSerializer;
            _saleOrderAppService = saleOrderAppService;
        }

        [HttpGet("json/serialize")]
        public async Task<string> JsonSerialize()
        {
            GetSaleOrderPagingRequest req = new GetSaleOrderPagingRequest
            {
                MaxResultCount = 1
            };
            ServiceResult<PagedResultDto<SaleOrderDto>> reqResult = await _saleOrderAppService.GetOrderPagingAsync(req);
            SaleOrderDto saleOrderDto = reqResult.Data.Items[0];
            return _jsonSerializer.Serialize(saleOrderDto);
        }

        // 入参字符串太长，所以使用Post
        [HttpPost("json/deserialize")]
        public SaleOrderDto JsonDeserialize(string json)
        {
            return _jsonSerializer.Deserialize<SaleOrderDto>(json);
        }



        [HttpGet("binary/serialize")]
        public async Task<byte[]> BinarySerialize()
        {
            GetSaleOrderPagingRequest req = new GetSaleOrderPagingRequest
            {
                MaxResultCount = 1
            };
            ServiceResult<PagedResultDto<SaleOrderDto>> reqResult = await _saleOrderAppService.GetOrderPagingAsync(req);
            SaleOrderDto saleOrderDto = reqResult.Data.Items[0];
            return _objectSerializer.Serialize(saleOrderDto);
        }

        // 入参字符串太长，所以使用Post
        [HttpPost("binary/deserialize")]
        public SaleOrderDto BinaryDeserialize(string binaryStr)
        {
            byte[] binary = Convert.FromBase64String(binaryStr);
            return _objectSerializer.Deserialize<SaleOrderDto>(binary);
        }

    }
}
