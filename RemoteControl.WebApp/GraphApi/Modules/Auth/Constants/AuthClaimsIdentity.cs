using System.Security.Claims;

namespace RemoteControl.WebApp.GraphApi.Modules.Auth.Constants
{
    public class AuthClaimsIdentity : ClaimsIdentity
    {
        public const string DefaultIdClaimType = "Id";
    }
}
