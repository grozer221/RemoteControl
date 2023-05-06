using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using RemoteControl.WebApp.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.Encodings.Web;

namespace RemoteControl.WebApp.Middlewares
{
    public class BasicAuthenticationOptions : AuthenticationSchemeOptions
    {
    }

    public class BasicAuthenticationHandler : AuthenticationHandler<BasicAuthenticationOptions>
    {
        public const string SchemeName = "SchemeName";
        private readonly AuthService authService;

        public BasicAuthenticationHandler(
            IOptionsMonitor<BasicAuthenticationOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            AuthService authService) : base(options, logger, encoder, clock)
        {
            this.authService = authService;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            string token = Request.Headers[HeaderNames.Authorization];
            var handler = new JwtSecurityTokenHandler();
            var validations = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AuthService.Token)),
                ValidateIssuer = false,
                ValidateAudience = false
            };
            try
            {
                var claimsPrincipal = handler.ValidateToken(authService.CleanBearerInToken(token), validations, out var tokenSecure);
                //var userId = claimsPrincipal.Claims.GetUserId();
                //var sessions = await sessionManager.GetAsync(t => t.UserId == userId);
                //if (!sessions.Any(t => t.Token == token))
                //    throw new Exception("Bad token");
                var ticket = new AuthenticationTicket(claimsPrincipal, new AuthenticationProperties { IsPersistent = false }, SchemeName);
                return AuthenticateResult.Success(ticket);
            }
            catch (Exception ex)
            {
                return AuthenticateResult.Fail(ex);
            }
        }
    }

}
