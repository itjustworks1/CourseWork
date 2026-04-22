using API.DTO.Auth;
using System.Security.Claims;
using Magaz_Stroitelya.NewDB;

namespace API.Servise.Auth
{
    public interface IAuthTokenService
    {
        Task<LoginResponse> CreateAccessTokenResponseAsync(User user);
        //(string rawToken, Refreshtoken entity) CreateRefreshTokenEntity(int userId, HttpRequest request, int? refreshLifetimeDaysOverride = null);
    }
}
