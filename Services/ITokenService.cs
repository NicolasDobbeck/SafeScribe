using SafeScribe.DTOs;
using SafeScribe.DTOs.Request;
using SafeScribe.Models;

namespace SafeScribe.Services
{
    public interface ITokenService
    {
        Task<User> RegisterAsync(UserRegisterDto registerDto);

        Task<string?> LoginAsync(LoginRequestDto loginDto);
    }
}