// <copyright file="UserMappingProfile.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace GameStore.BusinessLayer.Interfaces.Mappings
{
    using AutoMapper;
    using GameStore.BusinessLayer.Interfaces.Converters;
    using GameStore.BusinessLayer.Interfaces.DTO;
    using Microsoft.AspNetCore.Identity;

    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            this.CreateMap<IdentityUser, UserDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => GuidConverter.ConvertToGuid(src.Id)))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.UserName));
        }
    }
}
