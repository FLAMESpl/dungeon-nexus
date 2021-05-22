using Blazored.LocalStorage;
using DungeonNexus.ViewModel.Users;
using Microsoft.AspNetCore.Components;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Threading.Tasks;

namespace DungeonNexus.View.Home.Pages
{
    public partial class Index
    {
        private const string URI_TEMPLATE = "https://github.com/login/oauth/authorize?client_id=5d34f736a0546c59cae6&redirect_uri=http://localhost:5000/login&state={0}";

        [Inject]
        [AllowNull]
        private NavigationManager NavigationManager { get; set; }

        [Inject]
        [AllowNull]
        private ILocalStorageService LocalStorage { get; set; }

        [Inject]
        private User UserViewModel
        {
            get => ViewModel!;
            set => ViewModel = value;
        }

        private readonly Random random = new Random();

        private async Task Click()
        {
            var state = random.NextDouble().ToString(CultureInfo.InvariantCulture).Replace(".", "");
            await LocalStorage.SetItemAsync(User.AUTH_STATE_KEY, state);
            NavigationManager.NavigateTo(string.Format(URI_TEMPLATE, state));
        }

        protected override async Task OnInitializedAsync()
        {
            if (await UserViewModel.TryLogInFromStorage())
            {
                NavigationManager.NavigateTo("welcome");
            }

            await base.OnInitializedAsync();
        }
    }
}
