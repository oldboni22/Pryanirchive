using System.Security.Cryptography;

namespace AuthService.Domain.ValueObjects;

public sealed record HashedString
{
    private const int SaltSize = 16;
    
    private const int HashSize = 32; 
    
    private const int Iterations = 100_000; 
    
    private static readonly HashAlgorithmName Algorithm = HashAlgorithmName.SHA256;

    public string Value { get; init; }

    private HashedString(string value) => Value = value;
    
    public static HashedString Hash(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(SaltSize);
        var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, Algorithm, HashSize);
        
        var result = $"{Convert.ToHexString(salt)}:{Convert.ToHexString(hash)}";
        return new HashedString(result);
    }
    
    public bool Verify(string password)
    {
        var parts = Value.Split(':');
        
        var salt = Convert.FromHexString(parts[0]);
        var storedHash = Convert.FromHexString(parts[1]);

        var computedHash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, Algorithm, HashSize);
        
        return CryptographicOperations.FixedTimeEquals(computedHash, storedHash);
    }

    public static HashedString FromDatabase(string storedValue) => new(storedValue);
}
