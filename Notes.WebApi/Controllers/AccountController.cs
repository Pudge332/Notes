using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Notes.WebApi.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Notes.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly JWTSettings _options;

        public AccountController(IOptions<JWTSettings> optAccess)
        {
            _options = optAccess.Value;
        }

        [HttpGet("GetToken")]
        public string GetToken()
        {
            List<Claim> claims =
            [
                new Claim(ClaimTypes.Name, "Pudge"),
                new Claim("level", "123"),
                new Claim(ClaimTypes.Role, "Admin"),
            ];

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey));

            var jwt = new JwtSecurityToken(
                        issuer: _options.Issuer,
                        audience: _options.Audience,
                        claims: claims,
                        expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(1)),
                        notBefore: DateTime.UtcNow,
                        signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
                        );
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}
