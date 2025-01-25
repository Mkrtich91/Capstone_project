using AutoMapper;
using GameStore.BusinessLayer.Interfaces.ResponseDto;
using MongoDB.Entities.Converter;
using MongoDB.Entities.Entities;

public class CategoryMappingProfile : Profile
{
    public CategoryMappingProfile()
    {
        CreateMap<Category, GetGenreResponse>()
             .ForMember(dest => dest.Id, opt => opt.MapFrom(src => GuidConverter.ConvertToGuid(src.CategoryID)))
             .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.CategoryName))
             .ForMember(dest => dest.ParentGenreId, opt => opt.Ignore());
    }
}


