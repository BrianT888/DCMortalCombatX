using System;
using System.ServiceModel;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using GamingLobbyServer;
using System.Threading.Tasks;
using MortalCombatXLibrary;

namespace MortalCombatXClient
{
    /// <summary>
    /// Interaction logic for LoginScreen.xaml
    /// </summary>
    public partial class LoginScreen : Window
    {
        private GamingLobbyServerInterface foob;
        public string LoggedInUsername { get; private set; }
        public LoginScreen()
        {
            InitializeComponent();

            ChannelFactory<GamingLobbyServerInterface> foobFactory;
            NetTcpBinding tcp = new NetTcpBinding();

            //brian - don't think we need timeout timers right

            string url = "net.tcp://localhost:8200/GamingLobbyService";
            foobFactory = new ChannelFactory<GamingLobbyServerInterface>(tcp, url);
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

                    // Go to lobby screen
                    LobbyScreen lobbyScreen = new LobbyScreen(username, foob);
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
