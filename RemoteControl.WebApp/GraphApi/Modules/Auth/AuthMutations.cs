using GraphQL;
using GraphQL.Types;
using Microsoft.Net.Http.Headers;
using RemoteControl.Business.Models;
using RemoteControl.Repositories;
using RemoteControl.WebApp.Extensions;
using RemoteControl.WebApp.GraphApi.Modules.Auth.Constants;
using RemoteControl.WebApp.GraphApi.Modules.Auth.Types;
using RemoteControl.WebApp.Services;

namespace RemoteControl.WebApp.GraphApi.Modules.Auth
{
    public class AuthMutations : ObjectGraphType
    {
        public AuthMutations(IHttpContextAccessor httpContextAccessor, AuthService authService, UsersRepository usersRepository, SessionRepository sessionRepository)
        {
            Field<NonNullGraphType<AuthResponseType>, AuthResponse>()
                .Name("Register")
                .Argument<NonNullGraphType<RegisterInputType>, RegisterInput>("input", "Argument to register new User")
                .ResolveAsync(async context =>
                {
                    var input = context.GetArgument<RegisterInput>("input");

                    var user = await usersRepository.GetByEmailAsync(input.Email);

                    if (user != null)
                        throw new Exception($"User with email '{input.Email}' already exist.");

                    var userId = Guid.NewGuid();
                    var saltedPassword = input.Password + userId;
                    var newUser = await usersRepository.CreateAsync(new User
                    {
                        Email = input.Email,
                        Password = saltedPassword.CreateMD5WithSalt(out var salt),
                        Salt = salt,
                    });

                    var session = new Session
                    {
                        Token = authService.GenerateAccessToken(newUser.Id),
                        LastTimeOnline = DateTime.UtcNow,
                        UserId = newUser.Id,
                    };
                    session = await sessionRepository.CreateAsync(session);

                    return new AuthResponse()
                    {
                        Token = session.Token,
                        User = newUser,
                    };
                });

            Field<NonNullGraphType<AuthResponseType>, AuthResponse>()
                .Name("Login")
                .Argument<NonNullGraphType<LoginInputType>, LoginInput>("input", "Argument to login User")
                .ResolveAsync(async context =>
                {
                    var input = context.GetArgument<LoginInput>("input");

                    var user = await usersRepository.GetByEmailAsync(input.Email);
                    if (user == null)
                        throw new Exception("Email or password not valid.");

                    var saltedPassword = input.Password.CombiteWithSalt(user.Salt);
                    if (user.Password != saltedPassword.CreateMD5())
                        throw new Exception("Login or password not valid.");

                    var session = new Session
                    {
                        Token = authService.GenerateAccessToken(user.Id),
                        LastTimeOnline = DateTime.UtcNow,
                        UserId = user.Id,
                    };
                    session = await sessionRepository.CreateAsync(session);

                    return new AuthResponse()
                    {
                        Token = session.Token,
                        User = user,
                    };
                });

            Field<NonNullGraphType<BooleanGraphType>, bool>()
                .Name("Logout")
                .ResolveAsync(async context =>
                {
                    var userId = httpContextAccessor.HttpContext.User.Claims.GetUserId();
                    var token = httpContextAccessor.HttpContext.Request.Headers[HeaderNames.Authorization];
                    await sessionRepository.RemoveAsync(userId, token);
                    return true;
                })
                .AuthorizeWith(AuthPolicies.Authenticated);
        }
    }
}
