using FluentValidation;
using GraphQL.Server;
using RemoteControl.WebApp.GraphApi;
using RemoteControl.WebApp.GraphApi.Modules.Auth.Constants;
using RemoteControl.WebApp.Middlewares;
using System.Reflection;

namespace RemoteControl.WebApp.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddGraphQLApi(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddHttpContextAccessor();
            services.AddTransient<AppSchema>();
            services.AddGraphQLUpload();
            services
                 .AddGraphQL(options =>
                 {
                     options.EnableMetrics = true;
                     options.UnhandledExceptionDelegate = (context) =>
                     {
                         Console.WriteLine(context.Exception.StackTrace);
                         context.ErrorMessage = context.Exception.Message;
                     };
                 })
                 .AddSystemTextJson()
                 .AddGraphTypes(typeof(AppSchema), ServiceLifetime.Transient)
                 .AddGraphQLAuthorization(options =>
                 {
                     options.AddPolicy(AuthPolicies.Authenticated, p => p.RequireAuthenticatedUser());
                 });
            return services;
        }

        public static IServiceCollection AddJwtAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization();
            services
                 .AddAuthentication(BasicAuthenticationHandler.SchemeName)
                 .AddScheme<BasicAuthenticationOptions, BasicAuthenticationHandler>(BasicAuthenticationHandler.SchemeName, _ => { });
            return services;
        }
    }
}
