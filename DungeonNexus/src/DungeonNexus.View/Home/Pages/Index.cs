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
        private const string GITHUB_URI_TEMPLATE = "https://github.com/login/oauth/authorize?client_id=5d34f736a0546c59cae6&redirect_uri=http://localhost:5000/login/github&state={0}";
        private const string FACEBOOK_URI_TEMPLATE = "https://www.facebook.com/v10.0/dialog/oauth?client_id=2836112659984937&redirect_uri=http://localhost:5000/login/facebook&state={0}&scope=public_profile";

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

        private readonly Random random = new();

        private async Task ClickGithub()
        {
            await NavigateToExternalIdentityProvider(GITHUB_URI_TEMPLATE);
        }

        private async Task ClickFacebook()
        {
            await NavigateToExternalIdentityProvider(FACEBOOK_URI_TEMPLATE);
        }

        private async Task NavigateToExternalIdentityProvider(string templateWithState)
        {
            NavigationManager.NavigateTo(string.Format(templateWithState, await SetState()));
        }

        private async Task<string> SetState()
        {
            var state = random.NextDouble().ToString(CultureInfo.InvariantCulture).Replace(".", "");
            await LocalStorage.SetItemAsync(User.AUTH_STATE_KEY, state);
            return state;
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
