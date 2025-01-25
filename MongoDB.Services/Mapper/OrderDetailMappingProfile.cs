using AutoMapper;
using GameStore.BusinessLayer.Interfaces.DTO;
using GameStore.BusinessLayer.Interfaces.ResponseDto;
using MongoDB.Entities.Converter;
using MongoDB.Entities.Entities;

namespace MongoDB.Services.Mapper
{
    public class OrderDetailMappingProfile : Profile
    {
        public OrderDetailMappingProfile()
        {
            CreateMap<OrderDetail, GetOrderGameResponse>()
                .ForMember(dest => dest.GameId, opt => opt.MapFrom(src => GuidConverter.ConvertToGuid(src.ProductID)))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => (double)src.UnitPrice)) 
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.Discount, opt => opt.MapFrom(src => (int?)src.Discount)); 
        }
    }
}
