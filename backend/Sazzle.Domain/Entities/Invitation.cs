namespace Sazzle.Domain.Entities;

public enum InvitationStatus
{
    Pending,
    Accepted,
    Revoked,
    Expired
}

public class Invitation
{
    public Guid Id { get; private set; }

    public Guid OrganizationId { get; private set; }
    public string Email { get; private set; } = null!;
    public Guid RoleId { get; private set; }
    public Guid InvitedByUserId { get; private set; }
    public string Token { get; private set; } = null!;
    public InvitationStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime ExpiresAt { get; private set; }


    private Invitation()
    {
    }

    public Invitation(Guid organizationId, string email, Guid roleId, Guid invitedByUserId)
    {
        Id = Guid.NewGuid();
        OrganizationId = organizationId;
        Email = email;
        RoleId = roleId;
        InvitedByUserId = invitedByUserId;
        Token = Guid.NewGuid().ToString("N");
        Status = InvitationStatus.Pending;
        CreatedAt = DateTime.UtcNow;
        ExpiresAt = DateTime.UtcNow.AddDays(7);
    }

    public void Accept()
    {
        if (Status != InvitationStatus.Pending)
            throw new InvalidOperationException("Invitation is no longer valid.");

        if (DateTime.UtcNow > ExpiresAt)
        {
            Status = InvitationStatus.Expired;
            throw new InvalidOperationException("Invitation has expired");
        }

        Status = InvitationStatus.Accepted;
    }
}