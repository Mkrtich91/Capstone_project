using AutoMapper;
using GameStore.BusinessLayer.Interfaces.DTO;
using GameStore.BusinessLayer.Interfaces.ResponseDto;
using MongoDB.Entities.Converter;
using MongoDB.Entities.Entities;

namespace MongoDB.Services.Mapper
{
    public class OrderMappingProfile : Profile
    {
        public OrderMappingProfile()
        {
            CreateMap<Order, GetOrderResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => GuidConverter.ConvertToGuid(src.OrderID)))
            .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => GuidHelper.ToGuid(src.CustomerID)))
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.OrderDate));
        }
    }
}
