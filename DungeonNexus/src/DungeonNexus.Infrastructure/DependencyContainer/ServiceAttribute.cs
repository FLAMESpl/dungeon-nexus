using Microsoft.Extensions.DependencyInjection;
using System;

namespace DungeonNexus.Infrastructure.DependencyContainer
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class ServiceAttribute : Attribute
    {
        private readonly ServiceLifetime lifetime;

        public ServiceAttribute(ServiceLifetime lifetime)
        {
            this.lifetime = lifetime;
        }

        public void Register(Type annotatedType, IServiceCollection services)
        {
            services.Add(new ServiceDescriptor(annotatedType, annotatedType, lifetime));
        }
    }
}
