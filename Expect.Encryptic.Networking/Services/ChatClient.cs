using Expect.Encryptic.Networking.Interfaces;
using Expect.Encryptic.Secure.Interfaces;
using System.Net.Sockets;
using System.Text;

namespace Expect.Encryptic.Networking.Services
{
    public class ChatClient : IClient
    {
        public bool IsConnected { get; private set; }
        public string HostIp { get; private set; }
        public event EventHandler<string> MessageReceived;

        private readonly IEncryptionService _encryptionService;

        private TcpClient _client;
        private StreamReader _reader;
        private StreamWriter _writer;
        private bool _disconnect;

        public ChatClient(IEncryptionService encryptionService)
        {
            _encryptionService = encryptionService;
        }

        public async Task ConnectAsync(string ip, int port)
        {
            _client = new TcpClient();
            await _client.ConnectAsync(ip, port);

            IsConnected = _client.Connected;
            HostIp = ip;
            _disconnect = false;

            var networkStream = _client.GetStream();
            _reader = new StreamReader(networkStream, Encoding.UTF8);
            _writer = new StreamWriter(networkStream, Encoding.UTF8) { AutoFlush = true };

            _ = ListenForMessagesAsync(); 
        }

        public async Task SendMessageAsync(string message)
        {
            var encryptedMessage = _encryptionService.Encrypt(message);
            await _writer.WriteLineAsync(encryptedMessage);
        }

        public void Disconnect()
        {
            _client.Close();
            IsConnected = _client.Connected;
            _disconnect = true;
        }

        private async Task ListenForMessagesAsync()
        {
            while (true)
            {
                if (_disconnect)
                    break;
                try
                {
                    var message = await _reader.ReadLineAsync();
                    var decryptedMessage = _encryptionService.Decrypt(message);
                    MessageReceived?.Invoke(this, decryptedMessage);
                }
                catch
                {
                    Console.WriteLine("Disconnected from server...");
                    break;
                }
            }
        }

        
    }
}
