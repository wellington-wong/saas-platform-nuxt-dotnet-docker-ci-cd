using Sazzle.Domain.Entities;

namespace Sazzle.Application.Common.Interfaces;

public interface IInvitationRepository
{
    Task<Invitation?> GetByTokenAsync(string token);
    Task AddAsync(Invitation invitation);
    Task SaveChangesAsync();
}