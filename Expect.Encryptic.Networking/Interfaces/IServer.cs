namespace Expect.Encryptic.Networking.Interfaces
{
    public interface IServer
    {
        public Task StartAsync(string ip, int port);
        
    }
}
