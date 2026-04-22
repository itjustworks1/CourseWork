using API.DTO.Auth;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Magaz_Stroitelya.NewDB;

namespace API.Servise.Auth
{
    public class AuthTokenService : IAuthTokenService
    {
        private readonly _113526KrylovKursovaiContext _db;
        private readonly IConfiguration _config;

        public AuthTokenService(_113526KrylovKursovaiContext db, IConfiguration config)
        {
            _db = db;
            _config = config;
        }

        public async Task<LoginResponse> CreateAccessTokenResponseAsync(User user)
        {
            var issuer = _config["Jwt:Issuer"] ?? throw new InvalidOperationException("Не задан Jwt:Issuer.");
            //var audience = _config["Jwt:Audience"] ?? throw new InvalidOperationException("Не задан Jwt:Audience.");
            var key = _config["Jwt:Key"] ?? throw new InvalidOperationException("Не задан Jwt:Key.");
            var expiresInMinutes = int.Parse(_config["Jwt:ExpiresInMinutes"] ?? "120");

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Login),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var token = new JwtSecurityToken(
                issuer: issuer,
                //audience: audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(expiresInMinutes),
                signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
            );

            return new LoginResponse
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                UserId = user.Id
            };
        }
    }
}
