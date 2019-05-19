using AutoMapper;
using BLL.DTO;

namespace BLL.MapperConfigurations
{
    public class ProfileMapperConfiguration : MapperConfiguration
    {
        public ProfileMapperConfiguration() :
            base(config =>
            {
                config.CreateMap<DAL.Entities.Profile, ProfileDTO>();
                config.CreateMap<DAL.Entities.Profile, ProfileDTO>()
                    .ReverseMap()
                    .ForMember(destination => destination.Name, 
                        options => options.MapFrom(source => source.Name.Trim()));
            })
        { }
    }
}
