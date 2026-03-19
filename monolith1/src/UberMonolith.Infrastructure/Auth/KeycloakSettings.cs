namespace UberMonolith.Infrastructure;

public class KeycloakSettings
{
    public string BaseUrl { get; set; } = null!;      
    public string Realm { get; set; } = null!;        
    public string ClientId { get; set; } = null!;   
    public string ClientSecret { get; set; } = null!;
}
