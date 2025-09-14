using GamingLobbyServer;
using MortalCombatXLibrary;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MortalCombatXDuplexClient
{
    /// <summary>
    /// Interaction logic for LoginScreen.xaml
    /// </summary>
    public partial class LoginScreen : Window
    {
        private GamingLobbyServerDuplexInterface foob;
        private CallbackHandler callbackHandler;
        public string LoggedInUsername { get; private set; }
        public LoginScreen()
        {
            InitializeComponent();

            ChannelFactory<GamingLobbyServerDuplexInterface> foobFactory;
            NetTcpBinding tcp = new NetTcpBinding();

            // Create callback handler
            callbackHandler = new CallbackHandler();

            // Create duplex channel
            InstanceContext context = new InstanceContext(callbackHandler);
            string url = "net.tcp://localhost:8201/GamingLobbyDuplexService";
            foobFactory = new DuplexChannelFactory<GamingLobbyServerDuplexInterface>(context, tcp, url);
            foob = foobFactory.CreateChannel();
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = EnteredText.Text.Trim();
             

            if (!string.IsNullOrEmpty(username))
            {
                try
                {
                    LoginButton.IsEnabled = false;
                    LoadingBar.Visibility = Visibility.Visible;

                    await Task.Run(() => foob.Login(username));

                    // Register for callbacks
                    foob.RegisterForCallbacks(username);

                    // Go to lobby screen
                    LobbyScreen lobbyScreen = new LobbyScreen(username, foob, callbackHandler);
                    lobbyScreen.Show();
                    this.Close();
                }
                catch(FaultException<UsernameExistsFault> ex) 
                {
                    LoadingBar.Visibility = Visibility.Collapsed;
                    MessageBox.Show(ex.Detail.Message, "Login Error");
                    LoginButton.IsEnabled = true;
                }
            }
            else
            {
                MessageBox.Show("Enter username");
            }
        }

        private void ExitToDesktopButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
