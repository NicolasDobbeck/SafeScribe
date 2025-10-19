using SafeScribe.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SafeScribe.Middleware
{
    public class JwtBlacklistMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtBlacklistMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ITokenBlacklistService blacklistService)
        {

            if (!context.User.Identity.IsAuthenticated)
            {
                await _next(context);
                return;
            }

            var jtiClaim = context.User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti);

            if (jtiClaim == null)
            {
                await _next(context);
                return;
            }

            var jti = jtiClaim.Value;

            if (await blacklistService.IsBlacklistedAsync(jti))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Token inválido ou revogado.");
                return;
            }
            await _next(context);
        }
    }
}