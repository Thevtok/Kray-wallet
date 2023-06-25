using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Go_Saku.Net.Models;
using Microsoft.IdentityModel.Tokens;
public class TokenService
{
    private readonly SymmetricSecurityKey _secretKey;

    public TokenService(SymmetricSecurityKey secretKey)
    {
        _secretKey = secretKey;
    }

    public string GenerateToken(User user)
    {
        var claims = new[]
        {
            new Claim("email", user.Email),
            new Claim("password", user.Password),
            new Claim("username", user.Username),
            new Claim("user_id", user.ID.ToString()),
            new Claim("role", user.Role),
            new Claim("exp", DateTimeOffset.Now.AddHours(1).ToUnixTimeSeconds().ToString())
        };

        var credentials = new SigningCredentials(_secretKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: credentials
        );

        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(token);
    }
}
