using Expect.Encryptic.Message.Interfaces;
using Expect.Encryptic.Networking.Interfaces;
using Expect.Encryptic.Secure.Interfaces;
using System.Net.Sockets;
using System.Text;

namespace Expect.Encryptic.Networking.Services
{
    public class ChatClient(IMessageService messageService) : IClient
    {
        public bool IsConnected { get; private set; }
        public string HostIp { get; private set; }
        public event EventHandler<string> MessageReceived;

        private readonly IMessageService _messageService = messageService;

        private TcpClient _client;
        private StreamReader _reader;
        private StreamWriter _writer;
        private bool _disconnected;

        public async Task ConnectAsync(string ip, int port)
        {
            _client = new TcpClient();
            await _client.ConnectAsync(ip, port);

            IsConnected = true;
            HostIp = ip;
            _disconnected = false;

            var networkStream = _client.GetStream();
            _reader = new StreamReader(networkStream, Encoding.UTF8);
            _writer = new StreamWriter(networkStream, Encoding.UTF8) { AutoFlush = true };

            _ = ListenForMessagesAsync();
        }

        public void Disconnect()
        {
            _client.Close();
            IsConnected = _client.Connected;
            _disconnected = true;
        }

        public async Task SendMessageAsync(string message)
        {
            await _messageService.SendMessage(_writer, message);
        }

        private async Task ListenForMessagesAsync()
        {
            while (true)
            {
                if (_disconnected)
                    break;

                try
                {
                    await _messageService.ReciveMessage(_reader, MessageReceived);
                }
                catch
                {
                    await Console.Out.WriteLineAsync("Disconnected from server");
                    break;
                }
            }
        }
    }
}
