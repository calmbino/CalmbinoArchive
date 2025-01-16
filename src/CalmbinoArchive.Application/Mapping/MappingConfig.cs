using AutoMapper;
using CalmbinoArchive.Application.DTOs;
using CalmbinoArchive.Domain.Entities.Identity;

namespace CalmbinoArchive.Application.Mapping;

public class MappingConfig : Profile
{
    public MappingConfig()
    {
        CreateMap<LoginRequestDto, User>();
        CreateMap<User, CachedUser>();
    }
}