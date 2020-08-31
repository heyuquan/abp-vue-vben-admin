using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace Mk.DemoB.Dto.SaleOrders
{
    public class SaleOrderDetailUpdateDto : SaleOrderDetailCreateOrUpdateDtoBase
    {
        /// <summary>
        /// 有值为更新；null为新增
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// 是否删除记录
        /// </summary>
        public bool IsDelete { get; set; }
    }
}
