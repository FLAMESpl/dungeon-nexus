using Blazored.LocalStorage;
using DungeonNexus.Infrastructure.DependencyContainer;
using DungeonNexus.Model.Users;
using DungeonNexus.ViewModel.Users.Facebook;
using DungeonNexus.ViewModel.Users.GitHub;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.Mime;
using System.Threading.Tasks;

namespace DungeonNexus.ViewModel.Users
{
    [Scoped]
    public class User : ViewModelBase
    {
        public const string AUTH_STATE_KEY = "AUTH_STATE";

        private readonly HttpClient httpClient;
        private readonly ILocalStorageService localStorage;
        private readonly UsersRepository usersRepository;
        private readonly LoggedUserStore userStore;
        private readonly FileRepository fileRepository;

        public string Name { get; private set; } = string.Empty;
        public IdentityProvider IdentityProvider { get; private set; }
        public string? AvatarUrl { get; private set; }

        public User(
            HttpClient httpClient, 
            ILocalStorageService localStorage, 
            UsersRepository usersRepository, 
            LoggedUserStore userStore,
            FileRepository fileRepository)
        {
            this.httpClient = httpClient;
            this.localStorage = localStorage;
            this.usersRepository = usersRepository;
            this.userStore = userStore;
            this.fileRepository = fileRepository;
        }

        public async Task<bool> TryLogInFromStorage()
        {
            return !string.IsNullOrEmpty(Name = await userStore.FindLoggedUserName());
        }

        public async Task LogInWithGitHub(string code, string state)
        {
            await ConfirmState(state);

            var authRequest = new HttpRequestMessage(HttpMethod.Post, "https://github.com/login/oauth/access_token");
            authRequest.Headers.Accept.Add(new(MediaTypeNames.Application.Json));
#pragma warning disable CS8714 
            // The type cannot be used as type parameter in the generic type or method.
            // Nullability of type argument doesn't match 'notnull' constraint.
            authRequest.Content = new FormUrlEncodedContent(new Dictionary<string?, string?>
            {
                { "client_id", "5d34f736a0546c59cae6" },
                { "client_secret", "6fe1ba263ca70895ef407590ce4f18269fea1297" },
                { "request_uri", "http://localhost:5000/login" },
                { "code", code },
                { "state", state }
            });
#pragma warning restore CS8714

            var authResponse = await httpClient.SendAsync(authRequest);
            authResponse.EnsureSuccessStatusCode();
            var deserializedAuthResponse = await authResponse.Content.ReadFromJsonAsync<GitHubAuthenticationResponse>();

            var userRequest = new HttpRequestMessage(HttpMethod.Get, "https://api.github.com/user");
            userRequest.Headers.Accept.Add(new(MediaTypeNames.Application.Json));
            userRequest.Headers.UserAgent.Add(new ProductInfoHeaderValue("DungeonNexus", "0.1"));
            userRequest.Headers.TryAddWithoutValidation("Authorization", deserializedAuthResponse!.AuthorizationToken);
            var userResponse = await httpClient.SendAsync(userRequest);
            userResponse.EnsureSuccessStatusCode();
            var deserializedUserResponse = (await userResponse.Content.ReadFromJsonAsync<GitHubUserResponse>())!;

            await UpsertUser(new Model.Users.User(
                deserializedUserResponse.name,
                deserializedUserResponse.id.ToString(),
                IdentityProvider.GitHub,
                deserializedUserResponse.avatar_url));
        }

        public async Task LogInWithFacebook(string code, string state)
        {
            await ConfirmState(state);

            const string authRequestUrlTemplate = "https://graph.facebook.com/v10.0/oauth/access_token?client_id=2836112659984937&redirect_uri=http://localhost:5000/login/facebook&client_secret=5f862c4199ca5c6495b0629f5d796e91&code={0}";

            var authResponse = await httpClient.GetAsync(string.Format(authRequestUrlTemplate, code));
            authResponse.EnsureSuccessStatusCode();
            var authDeserializedResponse = await authResponse.Content.ReadFromJsonAsync<FacebookAuthenticationResponse>();

            var userResponse = await httpClient.GetAsync($"https://graph.facebook.com/v10.0/me?access_token={authDeserializedResponse!.access_token}");
            userResponse.EnsureSuccessStatusCode();
            var userDeserializedResponse = (await userResponse.Content.ReadFromJsonAsync<FacebookUserResponse>())!;

            var avatarResponse = await httpClient.GetAsync($"https://graph.facebook.com/v10.0/me/picture?access_token={authDeserializedResponse!.access_token}");
            userResponse.EnsureSuccessStatusCode();

            var avatarFilePath = $"./pics/fb/{userDeserializedResponse.id}.jpg";
            await fileRepository.Upload(await avatarResponse.Content.ReadAsStreamAsync(), avatarFilePath);

            await UpsertUser(new Model.Users.User(
                userDeserializedResponse.name,
                userDeserializedResponse.id.ToString(),
                IdentityProvider.Facebook,
                avatarFilePath[1..]));
        }

        private async Task UpsertUser(Model.Users.User user)
        {
            var userId = await usersRepository.Upsert(user);
            await userStore.SetLoggedUser(userId);
            Name = user.Name;
            IdentityProvider = user.IdentityProvider;
            AvatarUrl = user.AvatarUrl;
        }

        private async Task ConfirmState(string state)
        {
            var localState = await localStorage.GetItemAsStringAsync(AUTH_STATE_KEY);

            if (localState != state)
                throw new InvalidOperationException("Local state does not match with state received from identity provider.");
        }
    }
}
