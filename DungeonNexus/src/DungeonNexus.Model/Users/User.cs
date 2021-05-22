using System.Diagnostics.CodeAnalysis;

namespace DungeonNexus.Model.Users
{
    public class User
    {
        public long Id { get; private set; }
        [AllowNull]
        public string Name { get; private set; }
        [AllowNull]
        public string ExternalId { get; private set; }
        public IdentityProvider IdentityProvider { get; private set; }
        public string? AvatarUrl { get; private set; }

        private User()
        {
        }

        public User(string name, string externalId, IdentityProvider identityProvider, string? avatarUrl)
        {
            Name = name;
            ExternalId = externalId;
            IdentityProvider = identityProvider;
            AvatarUrl = avatarUrl;
        }

        public void Update(string name, string? avatarUrl)
        {
            Name = name;
            AvatarUrl = avatarUrl;
        }
    }
}
