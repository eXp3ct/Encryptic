using Expect.Encryptic.Secure.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace Expect.Encryptic.Secure.Services
{
    public class EncryptionService : IEncryptionService
    {
        private readonly string key = "key";

        public string Encrypt(string plainText)
        {
            // Простое шифрование, можно использовать более сложные алгоритмы
            byte[] data = Encoding.UTF8.GetBytes(plainText);
            using (var sha256 = SHA256.Create())
            {
                var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(key));
                for (int i = 0; i < data.Length; i++)
                    data[i] ^= hash[i % hash.Length];
            }
            return Convert.ToBase64String(data);
        }

        public string Decrypt(string cipherText)
        {
            byte[] data = Convert.FromBase64String(cipherText);
            using (var sha256 = SHA256.Create())
            {
                var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(key));
                for (int i = 0; i < data.Length; i++)
                    data[i] ^= hash[i % hash.Length];
            }
            return Encoding.UTF8.GetString(data);
        }
    }
}
