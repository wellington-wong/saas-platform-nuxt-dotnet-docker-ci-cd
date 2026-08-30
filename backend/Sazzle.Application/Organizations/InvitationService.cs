using Sazzle.Application.Common.Interfaces;
using Sazzle.Domain.Entities;


namespace Sazzle.Application.Organizations;

public class InvitationService
{
    private readonly IInvitationRepository _invitations;
    private readonly IOrganizationRepository _organizations;
    private readonly IUserRepository _users;

    public InvitationService(IInvitationRepository invitations, IOrganizationRepository organizations,
        IUserRepository users)
    {
        _invitations = invitations;
        _organizations = organizations;
        _users = users;
    }

    public async Task<Invitation> InviteAsync(Guid organizationId, string email, Guid roleId, Guid invitedByUserId)
    {
        var invitation = new Invitation(organizationId, email, roleId, invitedByUserId);
        await _invitations.AddAsync(invitation);
        await _invitations.SaveChangesAsync();
        return invitation;
        // NOTE: sending the actual email is out of the scope for now - log/return the token for manual testing
    }

    public async Task AcceptAsync(string token, Guid acceptingUserId)
    {
        var invitation = await _invitations.GetByTokenAsync(token)
                         ?? throw new InvalidOperationException("Invalid invitation token.");

        var user = await _users.GetByIdAsync(acceptingUserId)
                   ?? throw new InvalidOperationException("User not found.");

        if (!string.Equals(user.Email, invitation.Email, StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException("This invitation was sent to a different email address.");

        var organization = await _organizations.GetByIdAsync(invitation.OrganizationId)
                           ?? throw new InvalidOperationException("Organization not found.");



        invitation.Accept();
        var member = organization.AddMember(acceptingUserId, invitation.RoleId);

        await _organizations.AddMemberAsync(member);
        await _invitations.SaveChangesAsync();
    }
}