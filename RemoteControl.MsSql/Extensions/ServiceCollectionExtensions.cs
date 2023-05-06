using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace RemoteControl.MsSql.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMsSql(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<AppDbContext>((options) =>
            {
                options.UseSqlServer(connectionString ?? AppDbContext.DefaultConnectionString, b => b.MigrationsAssembly("RemoteControl.MsSql"));
            });

            return services;
        }
    }
}
