using UberMonolith.Models.Enums;
namespace UberMonolith.API.Models.Domains;

public class User
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public Role Role { get; set; }  

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public string PhoneNumber { get; set; } = null!;
    
}
