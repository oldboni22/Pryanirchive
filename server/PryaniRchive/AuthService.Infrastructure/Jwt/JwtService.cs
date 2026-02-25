using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AuthService.Application.Contracts;
using AuthService.Domain;
using Common.Authentication;
using Common.Logging;
using Common.ResultPattern;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AuthService.Infrastructure.Jwt;

public class JwtService(IOptions<JwtServiceOptions> options, ILogger<JwtService> logger) : IJwtService
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
        
        logger.LogTokenIssued(userId);

        return new TokenPair(handler.WriteToken(accessToken), Guid.NewGuid().ToString("N"));
    }
    
    //Left as a sample as for rn
    private Result<Guid> ExtractUserIdFromExpiredToken(string expiredToken)
    {
        logger.LogTokenValidationStarted();
        
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
            
            ValidateLifetime = false 
        };


        var principal = handler.ValidateToken(expiredToken, validationParameters, out var securityToken);

        if (securityToken is not JwtSecurityToken jwtToken ||
            !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
        {
            logger.LogTokenValidationFailed(new InvalidOperationException("Invalid token algorithm"));
            return DomainErrors.InvalidAccessToken;
        }

        var userIdClaim = principal.FindFirst(JwtRegisteredClaimNames.Sub);

        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
        {
            logger.LogTokenValidationFailed(new InvalidOperationException("Invalid user ID claim"));
            return DomainErrors.InvalidAccessToken;
        }

        logger.LogTokenValidationCompleted(userId);

        return userId;
    }
}
