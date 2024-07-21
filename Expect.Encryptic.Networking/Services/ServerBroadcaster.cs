using Expect.Encryptic.Networking.Interfaces;
using System.Net.Sockets;
using System.Text;

namespace Expect.Encryptic.Networking.Services
{
    public class ServerBroadcaster : IServerBroadcaster
    {
        private readonly IList<TcpClient> _clients = [];
        private readonly SemaphoreSlim _semaphore = new(1, 1);

        public async Task BroadcastMessageAsync(string message)
        {
            var buffer = Encoding.UTF8.GetBytes(message + "\n");

            await _semaphore.WaitAsync();

            try
            {
                var disconnectedCliens = new List<TcpClient>();

                foreach (var client in _clients)
                {
                    try
                    {
                        var stream = client.GetStream();

                        await stream.WriteAsync(buffer);
                        await stream.FlushAsync();
                    }
                    catch
                    {
                        disconnectedCliens.Add(client);
                    }
                }

                if (disconnectedCliens.Count > 0)
                    foreach (var client in disconnectedCliens)
                        _clients.Remove(client);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task AddClientAsync(TcpClient client)
        {
            await _semaphore.WaitAsync();

            try
            {
                _clients.Add(client);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task RemoveClientAsync(TcpClient client)
        {
            await _semaphore.WaitAsync();

            try
            {
                _clients.Remove(client);
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}
