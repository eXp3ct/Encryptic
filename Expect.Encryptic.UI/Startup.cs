using Expect.Encryptic.Message;
using Expect.Encryptic.Networking;
using Expect.Encryptic.Secure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Expect.Encryptic.UI
{
    public static class Startup
    {
        public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AppSettings>(configuration.GetSection(nameof(AppSettings)));
            services.AddMessageService();
            services.AddEncryption();
            services.AddNetworking();
            services.AddSingleton<MainWindow>();
        }
    }
}
