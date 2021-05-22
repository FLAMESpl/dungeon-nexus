using DungeonNexus.Infrastructure.DependencyContainer;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DungeonNexus.Model.Users
{
    [Scoped]
    public class UsersRepository
    {
        private readonly IDbContextFactory<DungeonNexusDB> dbContextFactory;

        public UsersRepository(IDbContextFactory<DungeonNexusDB> dbContextFactory)
        {
            this.dbContextFactory = dbContextFactory;
        }

        public async Task<User> Find(long id)
        {
            using var dbContext = dbContextFactory.CreateDbContext();
            return await dbContext.Users.FindAsync(id);
        }

        public async Task<long> Upsert(User user)
        {
            using var dbContext = dbContextFactory.CreateDbContext();

            var existingUser = await dbContext.Users.SingleOrDefaultAsync(x => 
                x.ExternalId == user.ExternalId && 
                x.IdentityProvider == user.IdentityProvider);

            if (existingUser is null)
            {
                dbContext.Add(user);
            }
            else
            {
                existingUser.Update(user.Name, user.AvatarUrl);
            }

            await dbContext.SaveChangesAsync();
            return existingUser?.Id ?? user.Id;
        }
    }
}
