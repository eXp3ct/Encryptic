using Expect.Encryptic.Networking.Interfaces;
using Expect.Encryptic.Secure.Interfaces;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Expect.Encryptic.Networking.Services
{
    public class ChatServer : IServer
    {
        private TcpListener _listener;
        private readonly List<TcpClient> _clients = new List<TcpClient>();
        private readonly SemaphoreSlim _clientsSemaphore = new SemaphoreSlim(1, 1);
        private readonly IEncryptionService _encryptionService;

        public ChatServer(IEncryptionService encryptionService)
        {
            _encryptionService = encryptionService;
        }

        public async Task StartAsync(string ip, int port)
        {
            _listener = new TcpListener(IPAddress.Parse(ip), port);
            _listener.Start();
            Console.WriteLine("Server started...");

            while (true)
            {
                var client = await _listener.AcceptTcpClientAsync();
                await _clientsSemaphore.WaitAsync();
                try
                {
                    _clients.Add(client);
                }
                finally
                {
                    _clientsSemaphore.Release();
                }
                Console.WriteLine("Client connected...");
                _ = HandleClientCommAsync(client); // Запуск без ожидания
            }
        }

        private async Task HandleClientCommAsync(TcpClient tcpClient)
        {
            var clientStream = tcpClient.GetStream();
            var reader = new StreamReader(clientStream, Encoding.UTF8);

            while (true)
            {
                try
                {
                    var message = await reader.ReadLineAsync();
                    var decryptedMessage = _encryptionService.Decrypt(message);
                    await BroadcastMessageAsync(decryptedMessage);
                }
                catch
                {
                    await _clientsSemaphore.WaitAsync();
                    try
                    {
                        _clients.Remove(tcpClient);
                    }
                    finally
                    {
                        _clientsSemaphore.Release();
                    }
                    tcpClient.Close();
                    Console.WriteLine("Client disconnected...");
                    break;
                }
            }
        }

        private async Task BroadcastMessageAsync(string message)
        {
            var encryptedMessage = _encryptionService.Encrypt(message);
            var buffer = Encoding.UTF8.GetBytes(encryptedMessage + "\n");

            await _clientsSemaphore.WaitAsync();
            try
            {
                var disconnectedClients = new List<TcpClient>();

                foreach (var client in _clients)
                {
                    try
                    {
                        var stream = client.GetStream();
                        await stream.WriteAsync(buffer, 0, buffer.Length);
                        await stream.FlushAsync();
                    }
                    catch
                    {
                        disconnectedClients.Add(client);
                    }
                }

                foreach (var client in disconnectedClients)
                {
                    _clients.Remove(client);
                }
            }
            finally
            {
                _clientsSemaphore.Release();
            }
        }
    }
}
