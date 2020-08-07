using System;
using System.Collections.Generic;
using System.Text;

namespace Mk.DemoB.Dto
{
    public class SequentialIdsDto
    {
        public List<string> Ids { get; set; }
            
        public SortedSet<string> SortIds { get; set; }
    }
}
