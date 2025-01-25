using AutoMapper;
using GameStore.BusinessLayer.Interfaces.DTO;
using GameStore.BusinessLayer.Interfaces.ResponseDto;
using MongoDB.Entities.Converter;
using MongoDB.Entities.Entities;
public class GameMappingProfile : Profile
{
    public GameMappingProfile()
    {
        CreateMap<Product, GetGameResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => GuidConverter.ConvertToGuid(src.ProductID)))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.ProductName))
            .ForMember(dest => dest.Key, opt => opt.MapFrom(src => src.GameKey))
            .ForMember(dest => dest.Description, opt => opt.Ignore())
            .ForMember(dest => dest.UnitInStock, opt => opt.MapFrom(src => src.UnitsInStock))
            .ForMember(dest => dest.Discount, opt => opt.MapFrom(src => src.Discontinued ? 0 : 100))
            .ForMember(dest => dest.PublisherId, opt => opt.MapFrom(src => GuidConverter.ConvertToGuid(src.SupplierID)))
            .ForMember(dest => dest.PublishedDate, opt => opt.Ignore())
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => (double)src.UnitPrice))
            .ForMember(dest => dest.ViewCount, opt => opt.MapFrom(src => src.ViewCount));
    }
}

