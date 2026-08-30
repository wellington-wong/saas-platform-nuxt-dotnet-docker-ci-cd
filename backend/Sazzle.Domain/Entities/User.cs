namespace Sazzle.Domain.Entities;

public class User

{
    public Guid Id { get; private set; }
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }
    public string FullName { get; private set; }
    public bool EmailConfirmed { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private readonly List<OrganizationMember> _memberships = new();

    public IReadOnlyCollection<OrganizationMember> Memberships => _memberships.AsReadOnly();
    
    private User() {}

    public User(string email, string passwordHash, string fullName)
    {
        Id = Guid.NewGuid();
        Email = email.ToLowerInvariant().Trim();
        PasswordHash = passwordHash;
        FullName = fullName;
        EmailConfirmed = false;
        CreatedAt = DateTime.UtcNow;
    }
    
    public void ConfirmEmail() => EmailConfirmed = true;

}