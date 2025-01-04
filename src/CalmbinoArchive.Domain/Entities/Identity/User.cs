using Microsoft.AspNetCore.Identity;

namespace CalmbinoArchive.Domain.Entities.Identity;

public class User : IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
}