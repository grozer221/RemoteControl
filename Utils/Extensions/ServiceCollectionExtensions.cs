using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using RemoteControl.WebApp.Attributes;
using System.Reflection;

namespace RemoteControl.WebApp.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInjectableServices(this IServiceCollection services)
        {
            var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
            var loadedPaths = loadedAssemblies.Select(a => a.Location).ToArray();
            var referencedPaths = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll");
            var toLoad = referencedPaths.Where(r => !loadedPaths.Contains(r, StringComparer.InvariantCultureIgnoreCase)).ToList();
            toLoad.ForEach(path => loadedAssemblies.Add(AppDomain.CurrentDomain.Load(AssemblyName.GetAssemblyName(path))));

            var implementationTypes = loadedAssemblies
                .SelectMany(asembly => asembly.GetTypes())
                .Where(type => type.IsDefined(typeof(InjectableServiceAttribute)));

            foreach (var implementationType in implementationTypes)
            {
                var injectableServiceAttribute = implementationType.GetCustomAttribute(typeof(InjectableServiceAttribute), true) as InjectableServiceAttribute;
                var serviceType = injectableServiceAttribute.ServiceType == null ? implementationType : injectableServiceAttribute.ServiceType;
                services.TryAdd(new ServiceDescriptor(serviceType, implementationType, injectableServiceAttribute.ServiceLifetime));
            }
            return services;
        }
    }
}
