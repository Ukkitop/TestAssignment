using TestAssignment.Application.Models;

namespace TestAssignment.Application.Interfaces;

public interface IAuthService
{
    Task<TokenInfo> RememberMeAsync(string code);
    string? ValidateToken(string token);
}

