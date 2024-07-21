using Expect.Encryptic.Networking.Interfaces;
using Expect.Encryptic.Networking.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Expect.Encryptic.Networking
{
    public static class DependencyInjection
    {
        public static void AddNetworking(this IServiceCollection services)
        {
            services.AddSingleton<IClientHandler, ClientHandler>();
            services.AddSingleton<IServerBroadcaster, ServerBroadcaster>();
            services.AddSingleton<IServer, ChatServer>();
            services.AddSingleton<IClient, ChatClient>();
        }
    }
}
