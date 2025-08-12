
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebAPITodoList.Models;
using WebAPITodoList.Settings;

namespace WebAPITodoList.Services;

public interface ITokenService
{
    string GenerateToken(User user);
    ClaimsPrincipal? ValidateToken(string token);
}
public class TokenService: ITokenService
{
    private readonly JwtSettings _jwtSettings;
    private readonly byte[] _key;
    public TokenService(IOptions<JwtSettings> jwtOptions)
    {
        _jwtSettings = jwtOptions.Value;
        _key = Encoding.UTF8.GetBytes(_jwtSettings.Key);
    }
    public string GenerateToken(User user)
    {
        List<Role> roles = [.. user.UserRoles.Select(s => s.Role)];
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Email),
            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        // Supponiamo che user.Roles sia una lista di nomi di ruoli
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role.RoleName));
        }

        // Create the security key and signing credentials
        var key = new SymmetricSecurityKey(_key);
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpireMinutes),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    public ClaimsPrincipal? ValidateToken(string token)
    {
        if (string.IsNullOrEmpty(token))
            return null;

        var tokenHandler = new JwtSecurityTokenHandler();
        try
        {
            var parameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = _jwtSettings.Issuer,
                ValidateAudience = true,
                ValidAudience = _jwtSettings.Audience,
                ValidateLifetime = true,
                IssuerSigningKey = new SymmetricSecurityKey(_key),
                ValidateIssuerSigningKey = true,
                ClockSkew = TimeSpan.Zero
            };

            var principal = tokenHandler.ValidateToken(token, parameters, out _);
            return principal;
        }
        catch
        {
            return null; // token invalide
        }
    }
}
