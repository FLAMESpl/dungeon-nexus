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
    }
}
