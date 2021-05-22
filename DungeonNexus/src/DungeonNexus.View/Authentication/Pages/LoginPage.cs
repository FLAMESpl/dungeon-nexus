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
            await UserViewModel.LogInWithGitHub(parsedQuery["code"], parsedQuery["state"]);
            NavigationManager.NavigateTo("welcome");
            await base.OnInitializedAsync();
        }
    }
}
