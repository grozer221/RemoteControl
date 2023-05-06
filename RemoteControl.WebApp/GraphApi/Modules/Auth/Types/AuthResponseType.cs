using GraphQL.Types;
using RemoteControl.Business.Models;
using RemoteControl.Repositories;
using RemoteControl.WebApp.GraphApi.Modules.Users;

namespace RemoteControl.WebApp.GraphApi.Modules.Auth.Types
{
    public class AuthResponseType : ObjectGraphType<AuthResponse>
    {
        public AuthResponseType(IServiceProvider serviceProvider)
        {
            Field<NonNullGraphType<UserType>, User>()
                .Name("User")
                .Resolve(context => context.Source.User);

            Field<NonNullGraphType<StringGraphType>, string>()
                .Name("Token")
                .Resolve(context => context.Source.Token);

            Field<NonNullGraphType<SessionType>, Session>()
                .Name("Session")
                .ResolveAsync(async context =>
                {
                    if (context.Source.Session != null)
                        return context.Source.Session;

                    using var scope = serviceProvider.CreateScope();
                    var sessionRepository = scope.ServiceProvider.GetRequiredService<SessionRepository>();
                    return await sessionRepository.GetByTokenAsync(context.Source.Token);
                });
        }
    }

    public class AuthResponse
    {
        public User User { get; set; }

        public string Token { get; set; }

        public Session Session { get; set; }
    }
}
