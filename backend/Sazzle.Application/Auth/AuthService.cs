using Sazzle.Application.Common.Interfaces;
using Sazzle.Domain.Entities;
namespace Sazzle.Application.Auth;

public class AuthService
{
    private readonly IUserRepository _users;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _tokenGenerator;

    public AuthService(IUserRepository users, IPasswordHasher passwordHasher, IJwtTokenGenerator tokenGenerator)
    {
        _users = users;
        _passwordHasher = passwordHasher;
        _tokenGenerator = tokenGenerator;
    }

    public async Task<User> RegisterAsync(string email, string password, string fullName)
    {
        var existing = await _users.GetByEmailAsync(email);
        if (existing is not null)
            throw new InvalidOperationException("Email is already registered");


        var hashedPassword = _passwordHasher.Hash(password);
        var user = new User(email, hashedPassword, fullName);

        await _users.AddAsync(user);
        await _users.SaveChangesAsync();

        return user;
    }


    public async Task<string> LoginAsync(string email, string password)
    {
        var user = await _users.GetByEmailAsync(email)
                   ?? throw new UnauthorizedAccessException("Invalid email or password.");

        if (!_passwordHasher.Verify(user.PasswordHash, password))
            throw new UnauthorizedAccessException("Invalid email or password.");

        return _tokenGenerator.GenerateToken(user);

    }
}