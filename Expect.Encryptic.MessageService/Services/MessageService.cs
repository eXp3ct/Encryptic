using Expect.Encryptic.Message.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expect.Encryptic.Message.Services
{
    public class MessageService : IMessageService
    {
        public event EventHandler<string> MessageReceived;

        public void SendMessage(string message)
        {
            MessageReceived?.Invoke(this, message);
        }
    }
}
