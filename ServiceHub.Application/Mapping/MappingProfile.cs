using AutoMapper;
using ServiceHub.Domain.Entities;
using ServiceHub.Application.DTOs;

namespace ServiceHub.Application.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<ApplicationUser, RegisterResponseDto>()
            .ForMember(dest => dest.EmailConfirmationToken, opt => opt.Ignore());
    }
}
