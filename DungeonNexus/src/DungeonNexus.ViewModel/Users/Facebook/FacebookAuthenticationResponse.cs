namespace DungeonNexus.ViewModel.Users.Facebook
{
    internal record FacebookAuthenticationResponse(string access_token, string token_type)
    {
        public string AuthenticationToken => $"{token_type} {access_token}";
    }
}
