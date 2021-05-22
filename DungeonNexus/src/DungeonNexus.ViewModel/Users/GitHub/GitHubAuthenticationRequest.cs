namespace DungeonNexus.ViewModel.Users.GitHub
{
    internal record GitHubAuthenticationRequest(string client_id, string client_secret, string code, string state, string request_uri);
}
