namespace Sazzle.Application.Organizations;

public record OrganizationMemberDetails(Guid UserId, string Email, string FullName, string RoleName, DateTime JoinedAt);