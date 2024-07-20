using Expect.Encryptic.Message.Interfaces;
using Expect.Encryptic.Message.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Expect.Encryptic.Message
{
    public static class DependencyInjection
    {
        public static void AddMessageService(this IServiceCollection services)
        {
            services.AddSingleton<IMessageService, MessageService>();
        }
    }
}
