﻿namespace DungeonNexus.ViewModel.Users.GitHub
{
    internal record GitHubAuthenticationResponse(string access_token, string scope, string token_type)
    {
        public string AuthorizationToken => $"{token_type} {access_token}";
    }
}
