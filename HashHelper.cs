using System.Security.Cryptography;

public static class HashHelper
{
public static string GenerateSalt(int length = 32)
{
    using var rng = RandomNumberGenerator.Create();
    byte[] saltBytes = new byte[length];
    rng.GetBytes(saltBytes);
    return Convert.ToBase64String(saltBytes);
}

/// <summary>
/// SHA-256 algoritması ile şifreyi hashler.
/// </summary>
public static string HashSha256(string password, string salt)
{
    using var sha256 = SHA256.Create();
    string saltedPassword = password + salt;
    byte[] hashBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(saltedPassword));
    return Convert.ToBase64String(hashBytes);
}

/// <summary>
/// SHA-256 algoritması ile hashlenmiş şifreyi doğrular.
/// </summary>
public static bool ValidateSha256(string password, string storedHash, string storedSalt)
{
    string hashedPassword = HashSha256(password, storedSalt);
    return hashedPassword == storedHash;
}
}