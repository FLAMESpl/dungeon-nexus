using Microsoft.Extensions.DependencyInjection;
using System;

namespace DungeonNexus.Infrastructure.DependencyContainer
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class SingletonAttribute : ServiceAttribute
    {
        public SingletonAttribute() : base(ServiceLifetime.Singleton)
        {
        }
    }
}
