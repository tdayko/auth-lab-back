using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using AuthLab.Application.IJwtService;
using AuthLab.Domain.Entities;

namespace AuthLab.Infra.JwtService;

internal class JwtService(IConfiguration configuration) : IJwtService
{
    private readonly IConfiguration _configuration = configuration;


    /// <summary>
    /// Generates a JWT (JSON Web Token) for the authenticated user.
    /// </summary>
    /// <param name="user">The user for whom the token is being generated.</param>
    /// <returns>A JWT as a string.</returns>
    public string CreateToken(User user)
    {
        List<Claim> claims = [
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Email, user.Email)
        ];

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("SecretyKey").Value!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddHours(4),
            signingCredentials: creds
        );

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);
        return jwt;
    }
}