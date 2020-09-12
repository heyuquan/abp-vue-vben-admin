using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Mk.DemoC.Dto.ElastcSearchs
{
    public class EsTestSearchRequest
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

        public decimal? MinPrice { get; set; }

        public decimal? MaxPrice { get; set; }
    }
}
