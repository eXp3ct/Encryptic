using Expect.Encryptic.Networking.Interfaces;
using System.Net;
using System.Net.Sockets;

namespace Expect.Encryptic.Networking.Services
{
    public class ChatServer(IClientHandler clientHandler, IServerBroadcaster serverBroadcaster) : IServer
    {
        private TcpListener? _listener;
        private readonly IClientHandler _clientHandler = clientHandler;
        private readonly IServerBroadcaster _serverBroadcaster = serverBroadcaster;

        public async Task StartAsync(string ip, int port)
        {
            var address = IPAddress.Parse(ip);
            _listener = new TcpListener(address, port);
            _listener.Start();
            await Console.Out.WriteLineAsync("Server started");

            while (true)
            {
                var client = await _listener.AcceptTcpClientAsync();
                await _serverBroadcaster.AddClientAsync(client);
                await Console.Out.WriteLineAsync("Client connected");

                _ = _clientHandler.HandleClientAsync(client);
            }
        }
    }
}
