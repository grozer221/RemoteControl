using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using RemoteControl.WebApp.Attributes;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RemoteControl.WebApp.Services
{
    [InjectableService]
    public class AuthService
    {
        public const string Bearer = "Bearer";
        public const string Token = "fergergpuoieqrhgueoirhngieurgergq";

        public string GenerateAccessToken(Guid userId)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Token));
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            List<Claim> claims = new List<Claim>
        {
            new Claim(ClaimsIdentity.DefaultNameClaimType, userId.ToString()),
        };
            var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.UtcNow.AddDays(30),
                    signingCredentials: signingCredentials);
            return Bearer + " " + new JwtSecurityTokenHandler().WriteToken(token);
        }

        public ClaimsPrincipal ValidateAccessToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(Token);
                tokenHandler.ValidateToken(CleanBearerInToken(token), new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                }, out var validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;

                ClaimsIdentity claimsIdentity = new ClaimsIdentity(jwtToken.Claims, JwtBearerDefaults.AuthenticationScheme);

                return new ClaimsPrincipal(claimsIdentity);
            }
            catch
            {
                return null;
            }
        }

        public string GenerateLoginToken()
        {
            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Token));
            SigningCredentials signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new JwtSecurityToken(
                    expires: DateTime.UtcNow.AddMinutes(30),
                    signingCredentials: signingCredentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public ClaimsPrincipal ValidateLoginToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(Token);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                }, out var validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;

                ClaimsIdentity claimsIdentity = new ClaimsIdentity(jwtToken.Claims, JwtBearerDefaults.AuthenticationScheme);

                return new ClaimsPrincipal(claimsIdentity);
            }
            catch
            {
                return null;
            }
        }

        public string? CleanBearerInToken(string token)
        {
            return token?.Replace(Bearer + " ", string.Empty);
        }
    }
}
