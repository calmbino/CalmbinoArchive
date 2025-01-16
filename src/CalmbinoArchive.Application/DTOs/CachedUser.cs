using CalmbinoArchive.Domain.Entities.Identity;

namespace CalmbinoArchive.Application.DTOs;

public class CachedUser : User
{
    public string RefreshToken { get; set; }
}