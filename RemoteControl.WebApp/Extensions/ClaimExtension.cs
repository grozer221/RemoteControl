using RemoteControl.WebApp.GraphApi.Modules.Auth.Constants;
using System.Security.Claims;

namespace RemoteControl.WebApp.Extensions;

public static class ClaimExtensions
{
    public static Guid GetUserId(this IEnumerable<Claim> claims)
    {
        return new Guid(claims.First(c => c.Type == AuthClaimsIdentity.DefaultIdClaimType).Value);
    }
}
