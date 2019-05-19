using AutoMapper;
using DAL.Entities;
using BLL.DTO;

namespace BLL.MapperConfigurations
{
    public class ImageFileMapperConfiguration : MapperConfiguration
    {
        public ImageFileMapperConfiguration() :
            base(config =>
            {
                config.CreateMap<File, ImageFileDTO>()
                    .ForMember(x => x.Name,
                        opt => opt.MapFrom(src => src.Name.Trim()))
                    .ForMember(x => x.FolderPath,
                        opt => opt.MapFrom(src => src.Folder.Trim()));
                config.CreateMap<ImageFileDTO, File>()
                    .ForMember(x => x.Folder,
                        opt => opt.MapFrom(src => src.FolderPath.Trim()));
            })
        { }
    }
}
