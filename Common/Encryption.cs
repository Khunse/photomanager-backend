using System.Security.Cryptography;

namespace imageuploadandmanagementsystem.Common
{
    public class Encryption
    {
        private const int _hashsize = 32;
        private const int _saltsize = 16;
        private const int _iterations = 100000;
        public static (string hashpass, string saltKey) Encrypt(string password)
        {
            var salt = new byte[_saltsize];
            RandomNumberGenerator.Fill(salt);
            var hash = new Rfc2898DeriveBytes(password, salt, _iterations, HashAlgorithmName.SHA256);
            var hashBytes = hash.GetBytes(_hashsize);

            return (Convert.ToBase64String(hashBytes), Convert.ToBase64String(salt));
        }

        public static bool ValidatePassword(string password, string hashpass, string saltKey)
        {
            var salt = Convert.FromBase64String(saltKey);
            var hash = Convert.FromBase64String(hashpass);
            var hashBytes = new Rfc2898DeriveBytes(password, salt, _iterations, HashAlgorithmName.SHA256).GetBytes(_hashsize);

            return CryptographicOperations.FixedTimeEquals(hash, hashBytes);
        }
        
    }
}