namespace Uber.Identity.API;
using System.Text.Json.Serialization;
public class CreateUser
{
    public string Username { get; set; }
    required public string Password { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Role UserRole { get; set; }
}
public enum Role { driver, rider }
