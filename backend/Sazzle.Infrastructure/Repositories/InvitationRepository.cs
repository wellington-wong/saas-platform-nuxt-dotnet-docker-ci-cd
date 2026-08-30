using Microsoft.EntityFrameworkCore;
using Sazzle.Application.Common.Interfaces;
using Sazzle.Domain.Entities;

using Sazzle.Infrastructure.Persistence;

namespace Sazzle.Infrastructure.Repositories;

public class InvitationRepository : IInvitationRepository
{
    private readonly SazzleDbContext _context;
    public InvitationRepository(SazzleDbContext context) => _context = context;


    public Task<Invitation?> GetByTokenAsync(string token) =>
        _context.Invitations.FirstOrDefaultAsync(i => i.Token == token);
    
    public async Task AddAsync(Invitation invitation) =>
        await _context.Invitations.AddAsync(invitation);

    public Task SaveChangesAsync() => _context.SaveChangesAsync();

}