using Expect.Encryptic.Message.Interfaces;
using Expect.Encryptic.Networking.Interfaces;
using Expect.Encryptic.Secure.Interfaces;
using Microsoft.Extensions.Options;
using System.Windows;

namespace Expect.Encryptic.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IServer _server;
        private readonly IClient _client;
        private readonly IOptions<AppSettings> _appSettings;

        private readonly IList<string> _connectedUsers;

        public MainWindow(
            IServer server,
            IClient client,
            IOptions<AppSettings> appSettings)
        {
            InitializeComponent();
            _server = server;
            _client = client;

            _client.MessageReceived += OnMessageRecived;
            _appSettings = appSettings;
            _connectedUsers = new List<string>();
        }

        private void OnMessageRecived(object? sender, string e)
        {
            Dispatcher.Invoke(() => TextField.Text += $"{e}\n");
        }

        private async void SendButton_Click(object sender, RoutedEventArgs e)
        {
            var message = InputBox.Text;

            if (string.IsNullOrEmpty(message)) return;

            await _client.SendMessageAsync(message);
            InputBox.Clear();
        }

        private async void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            await _server.StartAsync(_appSettings.Value.DefaultIp, _appSettings.Value.DefaultPort);
        }

        private async void JoinButton_Click(object sender, RoutedEventArgs e)
        {
            await _client.ConnectAsync(_appSettings.Value.DefaultIp, _appSettings.Value.DefaultPort);
            
            RoomName.Header = _client.HostIp;

            if (_client.IsConnected)
            {
                SendButton.IsEnabled = true;
                LeaveButton.IsEnabled = true;
            }
        }

        private void LeaveButton_Click(object sender, RoutedEventArgs e)
        {
            _client.Disconnect();
            if (!_client.IsConnected)
            {
                SendButton.IsEnabled = false;
                LeaveButton.IsEnabled = false;
            }
        }
    }
}