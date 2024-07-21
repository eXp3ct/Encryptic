using Expect.Encryptic.Networking.Interfaces;
using System.Net.Sockets;
using System.Text;

namespace Expect.Encryptic.Networking.Services
{
    public class ClientHandler(IServerBroadcaster broadcaster) : IClientHandler
    {
        public async Task HandleClientAsync(TcpClient client)
        {
            var clientStream = client.GetStream();
            var reader = new StreamReader(clientStream, Encoding.UTF8);

            while (true)
            {
                try
                {
                    var message = await reader.ReadLineAsync();
                    await broadcaster.BroadcastMessageAsync(message);
                }
                catch
                {
                    client.Close();
                    await Console.Out.WriteLineAsync("Client disconnected");
                    break;
                }
            }
        }
    }
}
