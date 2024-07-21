using System.Net.Sockets;

namespace Expect.Encryptic.Networking.Interfaces
{
    public interface IServerBroadcaster
    {
        public Task BroadcastMessageAsync(string message);
        public Task AddClientAsync(TcpClient client);
        public Task RemoveClientAsync(TcpClient client);
    }
}
