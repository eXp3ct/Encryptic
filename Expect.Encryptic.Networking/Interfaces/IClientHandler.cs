using System.Net.Sockets;

namespace Expect.Encryptic.Networking.Interfaces
{
    public interface IClientHandler
    {
        public Task HandleClientAsync(TcpClient client);
    }
}
