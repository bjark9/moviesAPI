using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace BlazorApp1.Components.Core{
public class User
{
    public User(string username, string password)
    {
        Username = username;
        HashedPassword = ConvertPasswordToHash(password);
    }
    public List<Movie> userFavorites = new();
    public IReadOnlyList<Movie> UserFavorites => userFavorites;
    public string Username {get;set;}
    public string HashedPassword {get; private set;} 

    public string ConvertPasswordToHash(string password)
    {
        // generate 128-bit salt using a sequence of cryptographically strong random bytes
        byte[] salt = RandomNumberGenerator.GetBytes(128/8);

        // derive a 256-bit subkey (use HMACSHA256 with 100,000 iterations)
        byte[] hash = KeyDerivation.Pbkdf2(
            password: password!,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 256/8
        );

        // Combine salt + hash so you can retrieve the salt later for verification
        byte[] combined = new byte[salt.Length + hash.Length];
        Buffer.BlockCopy(salt, 0, combined, 0, salt.Length);
        Buffer.BlockCopy(hash, 0, combined, salt.Length, hash.Length);

        return Convert.ToBase64String(combined);
    }

    public bool VerifyPassword(string inputPassword, string storedHash)
    {
        byte[] combined = Convert.FromBase64String(storedHash);

        // Extract the salt
        byte[] salt = new byte[16];
        Buffer.BlockCopy(combined, 0, salt, 0, 16);

        // Re-hash the input password using same salt
        byte[] hash = KeyDerivation.Pbkdf2(
            password: inputPassword,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 256/8
        );

        for (int i = 0; i < hash.Length; i++)
        {
            if (combined[i+16] != hash[i])
            {
                return false;
            }            
        }
        return true;
    }
}
}