using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Magaz_Stroitelya.NewDB;
using Microsoft.IdentityModel.Tokens;
using API.DTO.Auth;
using API.Servise.Password;
using API.Servise.Auth;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly _113526KrylovKursovaiContext db;
        private readonly IConfiguration configuration;
        private readonly IPasswordHasherService passwordHasherService;
        private readonly IAuthTokenService authTokenService;

        //private readonly IPasswordValidationService _passwordValidationService;
        //private readonly IAuthTokenService _authTokenService;
        public AuthController(_113526KrylovKursovaiContext db, IConfiguration configuration, IPasswordHasherService passwordHasherService, IAuthTokenService authTokenService)
        {
            this.db = db;
            this.configuration = configuration;
            this.passwordHasherService = passwordHasherService;
            this.authTokenService = authTokenService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await db.Users.FirstOrDefaultAsync(x => x.Login == request.Login);

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            {
                return Unauthorized("Неверный логин или пароль");
            }
            if (!passwordHasherService.Verify(request.Password, user.Password))
                return null;

            var token = GenerateJwtToken(user);

            return Ok(new LoginResponse()
            {
                Token = token,
                UserId = user.Id,
                //ExpiresIn = int.Parse(configuration["Jwt:ExpiresInMinutes"]) * 60 //
            });


            //var response = await CreateAuthResponseAsync(user, null);
            //return Ok(response);
        }

        [HttpPost("register")]
        public async Task<ActionResult<LoginResponse>> Register([FromBody] LoginRequest request)
        {
            if (await db.Users.AnyAsync(x => x.Login == request.Login))
                return Conflict("Логин уже занят");

            var user = new User
            {
                Login = request.Login,
                Password = passwordHasherService.Hash(request.Password),
            };

            db.Users.Add(user);
            await db.SaveChangesAsync();

            var access = await authTokenService.CreateAccessTokenResponseAsync(user);
            var response = new LoginResponse()
            {
                Token = access.Token,
                UserId = access.UserId,
                //ExpiresIn = token.ValidTo,
            };
            return Ok(response);
        }

        [HttpGet("me/{id:int}")]
        public async Task<ActionResult<UserResponse>> GetMe(int id)
        {
            var user = db.Users.FirstOrDefault(x => x.Id == id);

            if (user == null)
                return NotFound();

            return Ok(new UserResponse
            {
                Id = user.Id,
                Login = user.Login,
                IsAdmin = user.IsAdmin,
            });
        }
        //private async Task<LoginResponse> CreateAuthResponseAsync(User user, int? refreshLifetimeDaysOverride)
        //{
        //    var access = await _authTokenService.CreateAccessTokenResponseAsync(user);
        //    var (refreshTokenRaw, refreshTokenEntity) = _authTokenService.CreateRefreshTokenEntity(user.Id, Request, refreshLifetimeDaysOverride);

        //    _db.Refreshtokens.Add(refreshTokenEntity);
        //    await _db.SaveChangesAsync();

        //    return new LoginResponse
        //    {
        //        Token = access.Token,
        //        UserId = access.UserId,
        //    };
        //}

        private string GenerateJwtToken(User user)
        {
            var claims = new[] {new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())};

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expires = DateTime.UtcNow.AddMinutes(double.Parse(configuration["Jwt:ExpiresInMinutes"]));

            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
