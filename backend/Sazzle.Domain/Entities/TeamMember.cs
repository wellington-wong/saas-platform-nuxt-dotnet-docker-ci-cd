namespace Sazzle.Domain.Entities;

public class TeamMember

{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public Guid TeamId { get; private set; }
    public DateTime JoinedAt { get; private set; }
    
    private TeamMember() { }

    public TeamMember(Guid userId, Guid teamId)

    {
        Id = Guid.NewGuid();
        UserId = userId;
        TeamId = teamId;
        JoinedAt = DateTime.UtcNow;
    }
}