using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace Mk.DemoB.Dto
{
    public class GetBookListRequestDto: PagedAndSortedResultRequestDto
    {
        public decimal? MaxPrice { get; set; }

        public decimal? MinPrice { get; set; }
    }
}
