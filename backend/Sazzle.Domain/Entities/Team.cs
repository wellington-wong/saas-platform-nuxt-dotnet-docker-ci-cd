namespace Sazzle.Domain.Entities;

public class Team
{
    public Guid Id { get; private set; }
    public Guid OrganizationId { get; private set; }
    public string Name { get; private set; }
    private readonly List<TeamMember> _members = new();
    public IReadOnlyCollection<TeamMember>  Members => _members.AsReadOnly();

    private Team() { }

    internal Team(Guid organizationId, string name)
    {
        Id = Guid.NewGuid();
        OrganizationId = organizationId;
        Name = name;
    }
}