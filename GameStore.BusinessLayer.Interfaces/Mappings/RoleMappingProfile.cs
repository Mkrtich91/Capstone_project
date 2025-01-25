// <copyright file="RoleMappingProfile.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace GameStore.BusinessLayer.Interfaces.Mappings
{
    using AutoMapper;
    using GameStore.BusinessLayer.Interfaces.Converters;
    using GameStore.BusinessLayer.Interfaces.DTO;
    using GameStore.DataAccessLayer.Interfaces.Entities;

    public class RoleMappingProfile : Profile
    {
        public RoleMappingProfile()
        {
            this.CreateMap<UserRole, RoleDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => GuidConverter.ConvertToGuid(src.Id)))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
        }
    }
}