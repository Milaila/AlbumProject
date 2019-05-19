using AutoMapper;
using BLL.DTO;
using DAL.Entities;

namespace BLL.MapperConfigurations
{
    public class EvaluationMapperConfiguration : MapperConfiguration
    {
        public EvaluationMapperConfiguration() :
            base(config =>
            {
                config.CreateMap<Evaluation, EvaluationDTO>();
                config.CreateMap<Evaluation, EvaluationDTO>()
                    .ReverseMap();
            })
        { }
    }
}
