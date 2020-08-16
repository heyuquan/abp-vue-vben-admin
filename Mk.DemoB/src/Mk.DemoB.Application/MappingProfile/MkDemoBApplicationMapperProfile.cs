using AutoMapper;
using Mk.DemoB.Dto.DtoValid;
using Mk.DemoB.DtoValid;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mk.DemoB.MappingProfile
{
    public class MkDemoBApplicationMapperProfile : Profile
    {
        public MkDemoBApplicationMapperProfile()
        {
            // 应用层的Mapping一般是  domain中的实体 到 Dto 的转换

            CreateMap<Book, BookDto>()
                .MapExtraProperties();
        }
    }
}
