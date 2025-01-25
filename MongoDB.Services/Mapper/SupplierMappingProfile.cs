using AutoMapper;
using GameStore.BusinessLayer.Interfaces.DTO;
using GameStore.BusinessLayer.Interfaces.ResponseDto;
using MongoDB.Entities.Converter;
using MongoDB.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDB.Services.Mapper
{
    public class SupplierMappingProfile : Profile
    {
        public SupplierMappingProfile()
        {
            CreateMap<Supplier, GetPublisherResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => GuidConverter.ConvertToGuid(src.SupplierID)))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src =>
                    $"CompanyName: {src.CompanyName}, " +
                    $"ContactName: {src.ContactName}, " +
                    $"ContactTitle: {src.ContactTitle}, " +
                    $"Address: {src.Address}, " +
                    $"City: {src.City}"))
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.CompanyName))
                .ForMember(dest => dest.HomePage, opt => opt.MapFrom(src => src.HomePage));
        }
    }
}

