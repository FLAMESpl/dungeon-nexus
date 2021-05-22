using DungeonNexus.ViewModel.Users;
using Microsoft.AspNetCore.Components;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace DungeonNexus.View.Home.Pages
{
    public partial class WelcomePage
    {
        [Inject]
        private User UserViewModel
        {
            get => ViewModel!;
            set => ViewModel = value;
        }

        [Inject]
        [AllowNull]
        private LoggedUserStore UserStore { get; set; }

        [Inject]
        [AllowNull]
        private NavigationManager NavigationManager { get; set; }

        public async Task Logout()
        {
            await UserStore.RemoveLoggedUser();
            NavigationManager.NavigateTo("/");
        }
    }
}
