using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace Mk.DemoC.Dto.ElastcSearchs
{
    public class EsSearchRequest: PagedResultRequestDto
    {
        [Required]
        public string Keyword { get; set; }
        /// <summary>
        /// 品牌
        /// </summary>
        public string Brand { get; set; }
        /// <summary>
        /// 币别
        /// </summary>
        public string Currency { get; set; }

        public double? MinPrice { get; set; }

        public double? MaxPrice { get; set; }
    }
}
