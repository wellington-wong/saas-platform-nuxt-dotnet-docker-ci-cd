using Microsoft.AspNetCore.Identity;
using Sazzle.Application.Common.Interfaces;
using Sazzle.Domain.Entities;

namespace Sazzle.Infrastructure.Security;

public class PasswordHasherService : IPasswordHasher
{
    private readonly PasswordHasher<User> _hasher = new();

    public string Hash(string plainTextPassword) =>
        _hasher.HashPassword(null!, plainTextPassword); // null user is fine. PasswordHasher<T> doesn't use it internally


    public bool Verify(string hashedPassword, string providedPassword)
    {
        var result = _hasher.VerifyHashedPassword(null!, hashedPassword, providedPassword);
        return result == PasswordVerificationResult.Success;
    }
}