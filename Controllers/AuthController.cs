using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SafeScribe.DTOs;
using SafeScribe.DTOs.Request;
using SafeScribe.DTOs.Response;
using SafeScribe.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SafeScribe.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly ITokenBlacklistService _blacklistService;

        public AuthController(ITokenService tokenService, ITokenBlacklistService blacklistService)
        {
            _tokenService = tokenService;
            _blacklistService = blacklistService;
        }

        // --- Endpoint de Registro (Tarefa 3.1.b) ---
        // Rota: POST api/v1/auth/registrar
        [HttpPost("registrar")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto registerDto)
        {
            try
            {
                var user = await _tokenService.RegisterAsync(registerDto);
                return Ok(new { Message = "Usuário registrado com sucesso!", UserId = user.Id });
            }
            catch (ApplicationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Ocorreu um erro interno no servidor.", Details = ex.Message });
            }
        }

        // --- Endpoint de Login ---
        // Rota: POST api/v1/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginDto)
        {
            var token = await _tokenService.LoginAsync(loginDto);

            if (token == null)
            {
                return Unauthorized(new { Message = "Nome de usuário ou senha inválidos." });
            }

            return Ok(new LoginResponseDto(token));
        }


        // --- Endpoint de Logout---
        // Rota: POST api/v1/auth/logout
        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            var jtiClaim = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti);

            if (jtiClaim == null)
            {
                return BadRequest(new { Message = "Token inválido." });
            }

            var jti = jtiClaim.Value;

            await _blacklistService.AddToBlacklistAsync(jti);

            return Ok(new { Message = "Logout realizado com sucesso. O token foi invalidado." });
        }
    }
}