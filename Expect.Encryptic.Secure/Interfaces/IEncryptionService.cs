namespace Expect.Encryptic.Secure.Interfaces
{
    public interface IEncryptionService
    {
        public string Encrypt(string plainText);
        public string Decrypt(string cypherText);
    }
}
