using AutoMapper;
using DAL.Entities;
using BLL.DTO;

namespace BLL.MapperConfigurations
{
    public class ImageMapperConfiguration : MapperConfiguration
    {
        public ImageMapperConfiguration() :
            base(config =>
            {
                config.CreateMap<Image, ImageDTO>();
                config.CreateMap<Image, ImageDTO>().ReverseMap();
            })
        { }
    }
}
