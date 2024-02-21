using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Notes.WebApi.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Notes.Users;

namespace Notes.WebApi.Services
{
    public class JwtProvider
    {
        private readonly JWTSettings _options;

        public JwtProvider(IOptions<JWTSettings> optAccess)
        {
            _options = optAccess.Value;
        }

        public string GenerateToken(User user)
        {
            Claim[] claims = [new("userId", user.Id.ToString())];

            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey)),
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                signingCredentials: signingCredentials,
                expires: DateTime.UtcNow.AddHours(_options.ExpiresHours));

            var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenValue;
        }
    }
}
