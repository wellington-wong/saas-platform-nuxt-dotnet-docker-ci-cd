using Sazzle.Domain.Entities;

namespace Sazzle.Application.Common.Interfaces;

public interface IOrganizationRepository
{
    Task<Organization?> GetByIdAsync(Guid id);
    Task<Organization?> GetBySlugAsync(string slug);
    Task AddAsync(Organization organization);
    Task SaveChangesAsync();
}