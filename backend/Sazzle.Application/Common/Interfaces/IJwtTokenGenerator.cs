using Sazzle.Domain.Entities;

namespace Sazzle.Application.Common.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}