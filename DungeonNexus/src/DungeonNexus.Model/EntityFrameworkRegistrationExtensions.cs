using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using System;

namespace DungeonNexus.Model
{
    public static class EntityFrameworkRegistrationExtensions
    {
        public static IServiceCollection AddDbContextFactory(this IServiceCollection services)
        {
            return services.AddDbContextFactory<DungeonNexusDB>(ConfigureDbContext);
        }

        public static IServiceCollection AddDbContext(this IServiceCollection services)
        {
            return services.AddDbContext<DungeonNexusDB>(ConfigureDbContext);
        }

        private static void ConfigureDbContext(DbContextOptionsBuilder options)
        {
            options
                .UseMySql(
                    "server=localhost;user=root;password=;database=DungeonNexus",
                    new MariaDbServerVersion(new Version(10, 2, 36)),
                    mySqlOptions => mySqlOptions.CharSet(CharSet.Utf8Mb4).CharSetBehavior(CharSetBehavior.NeverAppend))
#if DEBUG
                .EnableSensitiveDataLogging()
#endif
                .EnableDetailedErrors();
        }
    }
}
