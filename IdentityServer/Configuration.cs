namespace IdentityServer;

public class Configuration
{
    public static IEnumerable<Client> Clients => new List<Client>
    {
        new()
        {
            ClientId = "ShoppingCartClient",
            AllowedGrantTypes = new[]
            {
                GrantType.ClientCredentials
            },
            ClientSecrets = new List<Secret>
            {
                new("secret".Sha256())
            },
            AllowedScopes =
            {
                "ShoppingCartAPI"
            }
        }
    };

    public static IEnumerable<ApiScope> ApiScopes => new List<ApiScope>
    {
        new("ShoppingCartAPI", "Shopping Cart API")
    };

    public static IEnumerable<ApiResource> ApiResources = new List<ApiResource>();

    public static IEnumerable<IdentityResource> IdentityResources = new List<IdentityResource>();

    public static IEnumerable<TestUser> TestUsers => new List<TestUser>();
}