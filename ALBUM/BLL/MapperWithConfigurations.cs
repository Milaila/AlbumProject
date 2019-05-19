using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using AutoMapper.Configuration;
using AutoMapper.Internal;
using DAL.Entities;
using BLL.DTO;

namespace BLL
{
    public static class MapperWithConfigurations
    {
        static MapperWithConfigurations()
        {
            var el =  new MapperConfiguration(configuration =>
            {
                configuration.AllowNullCollections = true;
                configuration.CreateMap<Product, ProductDTO>();
                configuration.CreateMap<ProductDTO, Product>();
                configuration.CreateMap<Supplier, SupplierDTO>();
                configuration.CreateMap<SupplierDTO, Supplier>();
                configuration.CreateMap<Category, CategoryDTO>();
                configuration.CreateMap<CategoryDTO, Category>();
            });
        }

        public static IMapper Instance
        {
            get => Mapper.Configuration.CreateMapper();
        }
    }
}
