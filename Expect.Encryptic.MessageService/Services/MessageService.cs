using Expect.Encryptic.Message.Interfaces;
using Expect.Encryptic.Secure.Interfaces;

namespace Expect.Encryptic.Message.Services
{
    public class MessageService(IEncryptionService encryptionService) : IMessageService
    {
        private readonly IEncryptionService _encryptionService = encryptionService;

        public async Task ReciveMessage(StreamReader reader, EventHandler<string> onMessageRecived)
        {
            var message = await reader.ReadLineAsync();
            if (message is null)
                return;

            var decryptedMessage = _encryptionService.Decrypt(message);

            onMessageRecived?.Invoke(this, decryptedMessage);
        }

        public async Task SendMessage(StreamWriter writer, string message)
        {
            var encryptedMessage = _encryptionService.Encrypt(message);
            await writer.WriteLineAsync(encryptedMessage);
        }
    }
}
