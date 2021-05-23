using DungeonNexus.ViewModel.Users;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonNexus.View.Layouts
{
    public partial class MainLayout
    {
        private bool initialized = false;

        [Inject]
        [AllowNull]
        private NavigationManager NavigationManager { get; set; }

        [Inject]
        [AllowNull]
        private User UserViewModel { get; set; }

        public async Task Logout()
        {
            await UserViewModel.LogOut();
            NavigationManager.NavigateTo("/");
        }

        protected override async Task OnInitializedAsync()
        {
            initialized = true;
            await base.OnInitializedAsync();
        }
    }
}
