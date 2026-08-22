namespace Sazzle.Application.Common.Interfaces;

public interface IPasswordHasher
{
    string Hash(string plainTextPassword);
    bool Verify(string hashedPassword, string providedPassword);
}