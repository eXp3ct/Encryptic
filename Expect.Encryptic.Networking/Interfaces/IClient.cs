namespace Expect.Encryptic.Networking.Interfaces
{
    public interface IClient
    {
        public bool IsConnected { get; }
        public string HostIp { get; }
        public event EventHandler<string> MessageReceived;
        public Task ConnectAsync(string ip, int port);
        public Task SendMessageAsync(string message);
        public void Disconnect();
    }
}
