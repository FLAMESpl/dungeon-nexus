using Microsoft.Extensions.DependencyInjection;
using System;

namespace DungeonNexus.Infrastructure.DependencyContainer
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class ScopedAttribute : ServiceAttribute
    {
        public ScopedAttribute() : base(ServiceLifetime.Scoped)
        {
        }
    }
}
