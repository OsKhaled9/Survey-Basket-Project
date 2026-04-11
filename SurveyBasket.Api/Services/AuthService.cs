using SurveyBasket.Api.Authentication;
using System.Security.Cryptography;

namespace SurveyBasket.Api.Services;

public class AuthService(UserManager<ApplicationUser> userManager, IJwtProvider jwtProvider) : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly IJwtProvider _jwtProvider = jwtProvider;

    private readonly int _refreshTokenExpiryDays = 14; // Example refresh token expiry time in days

    public async Task<AuthResponse?> GetTokenAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        // Check user?
        var user = await _userManager.FindByEmailAsync(email);

        if (user is null)
            return null;

        // Check password
        var isValidPassword = await _userManager.CheckPasswordAsync(user, password);

        if (!isValidPassword)
            return null;

        // Generate jwt token
        var (token, expiresIn) = _jwtProvider.GenerateToken(user);

        // Generate refresh token
        var refreshToken = GenerateRefreshToken();
        var refreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);

        // Store refresh token and its expiration in the database
        user.RefreshTokens.Add(new RefreshToken()
        {
            Token = refreshToken,
            ExpiresOn = refreshTokenExpiration,
        });

        await _userManager.UpdateAsync(user);

        // return new AuthResponse
        return new AuthResponse(Id: user.Id, Email: user.Email, FirstName: user.FirstName, LastName: user.LastName, token, expiresIn, refreshToken, refreshTokenExpiration);
    }

    public async Task<AuthResponse?> GetRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default)
    {
        var userId = _jwtProvider.ValidateToken(token);

        if (userId is null)
            return null;

        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
            return null;

        var userRefreshToken = user.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken && x.IsActive);

        if (userRefreshToken is null)
            return null;

        userRefreshToken.RevokedOn = DateTime.UtcNow;

        // Generate jwt token
        var (newtoken, expiresIn) = _jwtProvider.GenerateToken(user);

        // Generate refresh token
        var newRefreshToken = GenerateRefreshToken();
        var refreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);

        // Store refresh token and its expiration in the database
        user.RefreshTokens.Add(new RefreshToken()
        {
            Token = newRefreshToken,
            ExpiresOn = refreshTokenExpiration,
        });

        await _userManager.UpdateAsync(user);

        // return new AuthResponse
        return new AuthResponse(Id: user.Id, Email: user.Email, FirstName: user.FirstName, LastName: user.LastName, newtoken, expiresIn, newRefreshToken, refreshTokenExpiration);
    }

    public async Task<bool> RevokeRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default)
    {
        var userId = _jwtProvider.ValidateToken(token);

        if (userId is null)
            return false;

        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
            return false;

        var userRefreshToken = user.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken && x.IsActive);

        if (userRefreshToken is null)
            return false;

        userRefreshToken.RevokedOn = DateTime.UtcNow;

        await _userManager.UpdateAsync(user);

        return true;
    }

    private static string GenerateRefreshToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    }
}
