using Microsoft.Extensions.DependencyInjection;

namespace RemoteControl.WebApp.Attributes
{
    public class InjectableServiceAttribute : Attribute
    {
        private readonly ServiceLifetime serviceLifetime;
        private readonly Type? serviceType;

        public InjectableServiceAttribute(ServiceLifetime serviceLifetime = ServiceLifetime.Singleton, Type? serviceType = null)
        {
            this.serviceLifetime = serviceLifetime;
            this.serviceType = serviceType;
        }

        public ServiceLifetime ServiceLifetime
        {
            get => serviceLifetime;
        }

        public Type? ServiceType
        {
            get => serviceType;
        }
    }
}
