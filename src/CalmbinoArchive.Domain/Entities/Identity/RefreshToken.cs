namespace CalmbinoArchive.Domain.Entities.Identity;

public class RefreshToken
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string? UserId { get; set; }
    public string? Token { get; set; }
}