using AutoMapper;
using BLL.DTO;
using DAL.Entities;

namespace BLL.MapperConfigurations
{
    public class HashTagMapperConfiguration : MapperConfiguration
    {
        public HashTagMapperConfiguration() :
            base(config =>
            {
                config.CreateMap<HashTag, HashTagDTO>();
                config.CreateMap<HashTag, HashTagDTO>()
                    .ReverseMap()
                    .ForMember(destination => destination.Name, 
                        options => options.MapFrom(source => source.Name.Trim().ToUpper()));
            })
        { }
    }
}
