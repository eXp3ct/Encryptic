using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Configuration;
using System.Data;
using System.IO;
using System.Windows;

namespace Expect.Encryptic.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public IServiceProvider ServiceProvider { get; private set; }
        public IConfiguration Configuration { get; private set; }

        private readonly MainWindow _mainWindow;
        public App()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            Configuration = builder.Build();

            var serviceCollection = new ServiceCollection();
            serviceCollection.ConfigureServices(Configuration);
            serviceCollection.AddLogging();

            ServiceProvider = serviceCollection.BuildServiceProvider();
            _mainWindow = ServiceProvider.GetRequiredService<MainWindow>(); 
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            _mainWindow.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _mainWindow.Close();
            base.OnExit(e);
        }
    }

}
