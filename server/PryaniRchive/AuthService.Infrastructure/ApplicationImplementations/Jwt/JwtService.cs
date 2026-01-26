using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AuthService.Application.Contracts;
using AuthService.Domain;
using Common.ResultPattern;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

// Assuming Result is here

namespace AuthService.Infrastructure.ApplicationImplementations.Jwt;

public class JwtService(IOptions<JwtServiceOptions> options) : IJwtService
{
    private readonly JwtServiceOptions _options = options.Value;

    public TokenPair IssueTokens(Guid userId)
    {
        var handler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_options.SignatureKey);
        
        var accessTokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
            [
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            ]),
            Expires = DateTime.UtcNow.AddMinutes(_options.AccessTokenLifetimeMinutes),
            Issuer = _options.Issuer,
            Audience = _options.Audience,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key), 
                SecurityAlgorithms.HmacSha256Signature)
        };

        var accessToken = handler.CreateToken(accessTokenDescriptor);

        return new TokenPair(handler.WriteToken(accessToken), Guid.NewGuid().ToString("N"));
    }

    public Result<Guid> ExtractUserIdFromExpiredToken(string expiredToken)
    {
        var handler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_options.SignatureKey);

        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = _options.Issuer,
            ValidateAudience = true,
            ValidAudience = _options.Audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            
            // This is the "magic" for the refresh flow:
            // We verify the signature is ours, but ignore the expiration date.
            ValidateLifetime = false 
        };

        try
        {
            var principal = handler.ValidateToken(expiredToken, validationParameters, out var securityToken);

            if (securityToken is not JwtSecurityToken jwtToken || 
                !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                return DomainErrors.InvalidAccessToken;
            }
            
            var userIdClaim = principal.FindFirst(JwtRegisteredClaimNames.Sub);

            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
            {
                return DomainErrors.InvalidAccessToken;
            }

            return userId;
        }
        catch (Exception ex)
        {
            return ex;
        }
    }
}