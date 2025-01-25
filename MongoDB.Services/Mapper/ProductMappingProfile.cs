using AutoMapper;
using GameStore.BusinessLayer.Interfaces.DTO;
using MongoDB.Entities.Converter;
using MongoDB.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDB.Services.Mapper
{
    public class ProductMappingProfile : Profile
    {
        public ProductMappingProfile()
        {
            CreateMap<Product, GameOverviewDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => GuidConverter.ConvertToGuid(src.ProductID)))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.QuantityPerUnit))
                .ForMember(dest => dest.Key, opt => opt.MapFrom(src => src.GameKey))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.ProductName)) 
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => (double)src.UnitPrice)) 
                .ForMember(dest => dest.Discount, opt => opt.MapFrom(src => src.Discontinued ? 0 : 100))
                .ForMember(dest => dest.UnitInStock, opt => opt.MapFrom(src => src.UnitsInStock));
        }
    }
}
