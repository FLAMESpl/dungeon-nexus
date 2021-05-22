using DungeonNexus.ViewModel.Users;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace DungeonNexus.View.Authentication.Pages
{
    public partial class LoginPage
    {
        [Parameter]
        public string? IdentityProvider { get; set; }

        [Inject]
        private User UserViewModel
        {
            get => ViewModel!;
            set => ViewModel = value;
        }

        [Inject]
        [AllowNull]
        private NavigationManager NavigationManager { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var uri = new Uri(manager.Uri);
            var parsedQuery = QueryHelpers.ParseQuery(uri.Query);

            if (IdentityProvider == "github")
            {
                await UserViewModel.LogInWithGitHub(parsedQuery["code"], parsedQuery["state"]);
            }
            else if (IdentityProvider == "facebook")
            {
                await UserViewModel.LogInWithFacebook(parsedQuery["code"], parsedQuery["state"]);
            }
            else
            {
                throw new InvalidOperationException("Unknown identity provider.");
            }

            NavigationManager.NavigateTo("welcome");
            await base.OnInitializedAsync();
        }
    }
}
