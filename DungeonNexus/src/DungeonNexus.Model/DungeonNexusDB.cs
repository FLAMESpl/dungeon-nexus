using DungeonNexus.Model.Users;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace DungeonNexus.Model
{
    public class DungeonNexusDB : DbContext
    {
        [AllowNull] public DbSet<User> Users { get; set; }

        public DungeonNexusDB(DbContextOptions options) : base(options)
        {
        }
    }
}
