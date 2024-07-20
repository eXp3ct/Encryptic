using Expect.Encryptic.Secure.Interfaces;
using Expect.Encryptic.Secure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Expect.Encryptic.Secure
{
    public static class DependencyInjection
    {
        public static void AddEncryption(this IServiceCollection services)
        {
            services.AddSingleton<IEncryptionService, EncryptionService>();
        }
    }
}
