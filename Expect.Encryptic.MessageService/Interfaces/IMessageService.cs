namespace Expect.Encryptic.Message.Interfaces
{
    public interface IMessageService
    {
        public void SendMessage(string message);
        public event EventHandler<string> MessageReceived;
    }
}
