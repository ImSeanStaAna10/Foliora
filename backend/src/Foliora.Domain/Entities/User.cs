namespace Foliora.Domain.Entities;

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? DeletedAt { get; set; }

    public ICollection<ExternalIdentity> ExternalIdentities { get; set; } = new List<ExternalIdentity>();
}

public class ExternalIdentity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public string Provider { get; set; } = string.Empty; // e.g., 'google'
    public string SubjectId { get; set; } = string.Empty; // provider unique ID
    public DateTimeOffset ConnectedAt { get; set; } = DateTimeOffset.UtcNow;
}
