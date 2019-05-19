using BLL.Services;
using Ninject.Modules;
using AutoMapper;
using BLL.Interfaces;
using System;
using System.Collections.Generic;
using BLL.MapperConfigurations;

namespace DI
{
    public class BLLConfigurationModel : NinjectModule
    {
        public override void Load()
        {
            Bind<IProfileService>()
                .To<ProfileService>()
                .WithConstructorArgument<MapperConfiguration>
                    (new ProfileMapperConfiguration());
            Bind<IImageService>()
                .To<ImageService>()
                .WithConstructorArgument<MapperConfiguration>
                    (new ImageMapperConfiguration());
            Bind<IImageFileService>()
                .To<ImageFileService>()
                .WithConstructorArgument<MapperConfiguration>(new ImageFileMapperConfiguration())
                .WithConstructorArgument
                    (new Lazy<List<string>>(() => new List<string> { ".png", ".jpg", ".gif" }));
            Bind<IHashTagService>()
                .To<HashTagService>()
                .WithConstructorArgument<MapperConfiguration>
                    (new HashTagMapperConfiguration());
            Bind<IEvaluationService>()
                .To<EvaluationService>()
                .WithConstructorArgument<MapperConfiguration>
                    (new EvaluationMapperConfiguration());
        }
    }
}
