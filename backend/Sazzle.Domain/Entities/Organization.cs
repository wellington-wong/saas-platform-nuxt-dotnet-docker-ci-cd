namespace Sazzle.Domain.Entities;

public class Organization

{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Slug { get; private set; }
    public DateTime CreatedAt { get; private set; }
    private readonly List<OrganizationMember> _members = new();
    public IReadOnlyCollection<OrganizationMember> Members => _members.AsReadOnly();
    private readonly List<Team> _teams = new();
    public IReadOnlyCollection<Team> Teams => _teams.AsReadOnly();

    private Organization() { }

    public Organization(string name, string slug)
    {
        Id = Guid.NewGuid();
        Name = name;
        Slug =  slug;
        CreatedAt = DateTime.UtcNow;
    }

    public Team AddTeam(string teamName)
    {
        var team = new Team(Id, teamName);
        _teams.Add(team);
        return team;
    }

	public OrganizationMember AddMember(Guid userId, Guid roleId)
	{
   
		var member = new OrganizationMember(userId, Id, roleId);
		_members.Add(member);
		return member;

	}
}