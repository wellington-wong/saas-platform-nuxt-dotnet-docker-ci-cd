namespace Sazzle.Domain.Entities;

public class OrganizationMember

{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public Guid OrganizationId { get; private set; }
    public Guid RoleId { get; private set; }
    public DateTime JoinedAt { get; private set; }
    
    private OrganizationMember() { }

    
    public OrganizationMember(Guid userId, Guid organizationId, Guid roleId)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        OrganizationId = organizationId;
        RoleId = roleId;
        JoinedAt = DateTime.UtcNow;
    }

}