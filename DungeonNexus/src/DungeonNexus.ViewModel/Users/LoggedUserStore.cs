using Blazored.LocalStorage;
using DungeonNexus.Infrastructure.DependencyContainer;
using DungeonNexus.Model.Users;
using System.Threading.Tasks;

namespace DungeonNexus.ViewModel.Users
{
    [Scoped]
    public class LoggedUserStore
    {
        private const string USER_ID_KEY = "LOGGED_USER_ID";

        private readonly ILocalStorageService localStorage;
        private readonly UsersRepository usersRepository;

        public LoggedUserStore(ILocalStorageService localStorage, UsersRepository usersRepository)
        {
            this.localStorage = localStorage;
            this.usersRepository = usersRepository;
        }

        public async Task SetLoggedUser(long userId)
        {
            await localStorage.SetItemAsync(USER_ID_KEY, userId);
        }

        public async Task<string?> FindLoggedUserName()
        {
            var userId = await localStorage.GetItemAsync<long?>(USER_ID_KEY);

            if (userId is not null)
            {
                var user = await usersRepository.Find(userId.Value);
                if (user is null)
                {
                    await localStorage.RemoveItemAsync(USER_ID_KEY);
                    return null;
                }
                else
                {
                    return user.Name;
                }
            }
            else
            {
                return null;
            }
        }
    }
}
