using GraphQL;
using GraphQL.Types;
using Microsoft.Net.Http.Headers;
using RemoteControl.Business.Models;
using RemoteControl.Repositories;
using RemoteControl.WebApp.Extensions;
using RemoteControl.WebApp.GraphApi.Modules.Auth.Constants;
using RemoteControl.WebApp.GraphApi.Modules.Auth.Types;

namespace RemoteControl.WebApp.GraphApi.Modules.Auth
{
    public class AuthQueries : ObjectGraphType
    {
        public AuthQueries(IHttpContextAccessor httpContextAccessor, UsersRepository usersRepository, SessionRepository sessionRepository)
        {
            Field<NonNullGraphType<AuthResponseType>, AuthResponse>()
                .Name("Me")
                .ResolveAsync(async context =>
                {
                    var userId = httpContextAccessor.HttpContext.User.Claims.GetUserId();
                    return new AuthResponse()
                    {
                        Token = httpContextAccessor.HttpContext.Request.Headers[HeaderNames.Authorization],
                        User = await usersRepository.GetByIdAsync(userId),
                    };
                })
                .AuthorizeWith(AuthPolicies.Authenticated);

            Field<NonNullGraphType<ListGraphType<SessionType>>, IEnumerable<Session>>()
                .Name("GetSessions")
                .ResolveAsync(async context =>
                {
                    var userId = httpContextAccessor.HttpContext.User.Claims.GetUserId();
                    return await sessionRepository.GetAsync(s => s.UserId == userId);
                })
                .AuthorizeWith(AuthPolicies.Authenticated);
        }
    }
}
