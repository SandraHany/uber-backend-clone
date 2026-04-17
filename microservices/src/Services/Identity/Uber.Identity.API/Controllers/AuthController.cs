using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Net.Http.Headers;
using System.Text;
using DotNetEnv;
namespace Uber.Identity.API;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly ILogger<AuthController> _logger;
    private readonly IConfiguration _configuration;

    public AuthController(ILogger<AuthController> logger, IConfiguration configuration)
    {   
        _logger = logger;
        _configuration = configuration;
    }
    async Task<string> GetRoleId(string role)
    {
                var token = await GetAdminToken();
        var URL = $"http://localhost:8080/admin/realms/uber/roles/{role}";
        using (var client = new HttpClient())
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        
            var response = await client.GetAsync(URL);
            if (response.IsSuccessStatusCode)
            {
                 var json = await response.Content.ReadAsStringAsync();
                using var document = JsonDocument.Parse(json);
                string id = document.RootElement.GetProperty("id").GetString();
                return id;
             
            }
            else
            {
                 var errorBody = await response.Content.ReadAsStringAsync();
                throw new Exception($"Keycloak responded with: {errorBody}");
           
            }
        }
    }
    async Task<string> GetUserId(string username)
    {
        var token = await GetAdminToken();
        var URL = $"http://localhost:8080/admin/realms/uber/users?username={username}&exact=true";
        using (var client = new HttpClient())
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        
            var response = await client.GetAsync(URL);
            if (response.IsSuccessStatusCode)
            {
                 var json = await response.Content.ReadAsStringAsync();
                using var document = JsonDocument.Parse(json);
                string id = document.RootElement[0].GetProperty("id").GetString();
                return id;
             
            }
            else
            {
                 var errorBody = await response.Content.ReadAsStringAsync();
                throw new Exception($"Keycloak responded with: {errorBody}");
           
            }
        }
        
        
    }
    string GetTokenURL()
    {
        var tokenURL = "http://localhost:8080/realms/uber/protocol/openid-connect/token";
        return tokenURL;
    }
    string GetRegisterURL()
    {
        string registerURL = "http://localhost:8080/admin/realms/uber/users";
        return registerURL;
    }
    string GetClientSecret()
    {
        var clientSecret = _configuration["KEYCLOAK_SECRET"];
        return clientSecret;
    }
    async Task<string> GetAdminToken()
    {
        var tokenURL = GetTokenURL();
        var dict = new Dictionary<string, string>();
        dict.Add("client_id", "auth-client");
        dict.Add("client_secret", GetClientSecret());
        dict.Add("grant_type", "client_credentials");

        using (var client = new HttpClient())
        {
            var content = new FormUrlEncodedContent(dict);
            var response = await client.PostAsync(tokenURL, content);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                using var document = JsonDocument.Parse(json);
                string token = document.RootElement.GetProperty("access_token").GetString();
                return token;
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }
        }
    }
        async Task<string> GetRefreshToken(string refreshToken)
    {
        var tokenURL = GetTokenURL();
        var dict = new Dictionary<string, string>();
        dict.Add("client_id", "auth-client");
        dict.Add("client_secret", GetClientSecret());
        dict.Add("grant_type", "refresh_token");
        dict.Add("refresh_token", refreshToken);

        using (var client = new HttpClient())
        {
            var content = new FormUrlEncodedContent(dict);
            var response = await client.PostAsync(tokenURL, content);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                using var document = JsonDocument.Parse(json);
                string token = document.RootElement.GetProperty("access_token").GetString();
                return token;
            }
            else
            {
                var errorBody = await response.Content.ReadAsStringAsync();
                throw new Exception($"Keycloak responded with: {errorBody}");
            }
        }
    }
    async Task<(string token, string refreshToken)> GetPasswordToken(string username, string password)
    {
        var tokenURL = GetTokenURL();
        var dict = new Dictionary<string, string>();
        dict.Add("client_id", "auth-client");
        dict.Add("client_secret", GetClientSecret());
        dict.Add("grant_type", "password");
        dict.Add("username", username);
        dict.Add("password", password);

        using (var client = new HttpClient())
        {
            var content = new FormUrlEncodedContent(dict);
            var response = await client.PostAsync(tokenURL, content);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                using var document = JsonDocument.Parse(json);
                string token = document.RootElement.GetProperty("access_token").GetString();
                string refreshToken = document.RootElement.GetProperty("refresh_token").GetString();
                return (token, refreshToken);
            }
            else
            {
                var errorBody = await response.Content.ReadAsStringAsync();
    throw new Exception($"Keycloak responded with: {errorBody}");
            }
        }
    }
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var (token, refreshToken) = await GetPasswordToken(request.Username, request.Password);

        return Ok(new{token,refreshToken});
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] CreateUser createUser)
    {
      var token = await GetAdminToken();
      _logger.LogInformation("Token obtained: {Token}", token);
      var registerURL = GetRegisterURL();
      using (var client = new HttpClient())
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
           var user = new
            {
                username = createUser.Username,
                email = createUser.Email,
                enabled = true,
                firstName = createUser.FirstName,
                lastName = createUser.LastName,
                emailVerified = true, 
                requiredActions = new string[] { },
                realmRoles = new[] { createUser.UserRole.ToString() },
                credentials = new[]
                {
                    new { type = "password", value = createUser.Password, temporary = false }
                }
            };
            var content = new StringContent(JsonSerializer.Serialize(user), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(registerURL, content);
            if (response.IsSuccessStatusCode)
            {
                return Ok("Registration successful");
            }
            else
            {
                var errorDetails = await response.Content.ReadAsStringAsync();
                throw new Exception($"Keycloak responded with: {errorDetails}");
            }
        }
    }
    [HttpPut("update-profile")]
    public IActionResult UpdateProfile()
    {
        // Implement profile update logic here
        return Ok("Profile updated successfully");
    }
    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken(string oldRefreshToken)
    {
        var newToken = await GetRefreshToken(oldRefreshToken);
        return Ok(newToken);
    }
    [HttpPost("assign-role")]
    public async Task<IActionResult> AssignRole([FromBody] AssignRoleRequest request)
    {
        var userId = await GetUserId(request.Username);
        var roleId = await GetRoleId(request.Role.ToString());
        var token = await GetAdminToken();
        var url = $"http://localhost:8080/admin/realms/uber/users/{userId}/role-mappings/realm";
 using (var client = new HttpClient())
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
           var req = new []
           {
               new
                {
                    id = roleId,
                    name = request.Role.ToString()
                }
           };
            var content = new StringContent(JsonSerializer.Serialize(req), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, content);
            if (response.IsSuccessStatusCode)
            {
                return Ok("Role update successful");
            }
            else
            {
                var errorDetails = await response.Content.ReadAsStringAsync();
                throw new Exception($"Keycloak responded with: {errorDetails}");
            }
        }
    }
    
}

