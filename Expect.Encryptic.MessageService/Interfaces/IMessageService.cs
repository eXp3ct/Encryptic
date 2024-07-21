namespace Expect.Encryptic.Message.Interfaces
{
    public interface IMessageService
    {
        public Task SendMessage(StreamWriter writer, string message);
        public Task ReciveMessage(StreamReader reader, EventHandler<string> onMessageRecived);
    }
}
