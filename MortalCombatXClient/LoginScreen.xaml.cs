using System.ServiceModel;
using System.Windows;
using System.Windows.Controls;
using GamingLobbyServer;

namespace MortalCombatXClient
{
    /// <summary>
    /// Interaction logic for LoginScreen.xaml
    /// </summary>
    public partial class LoginScreen : Window
    {
        private GamingLobbyServerInterface foob;
        public LoginScreen()
        {
            InitializeComponent();

            ChannelFactory<GamingLobbyServerInterface> foobFactory;
            NetTcpBinding tcp = new NetTcpBinding();

            //brian - don't think we need timeout timers right

            string url = "net.tcp://localhost:8200/GamingLobbyServer";
            foobFactory = new ChannelFactory<GamingLobbyServerInterface>(tcp, url);
            foob = foobFactory.CreateChannel();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = EnteredText.Text;
            if (!string.IsNullOrEmpty(username))
            {
                //bool unique = client.Login(username);
                //if (unique)
                //{
                //    MessageBox.Show("Login was successful");
                   
                //}
                //else
                //{
                //    MessageBox.Show("Username is taken, choose different username");
                //}

                //brian - i assume any username is valid, so special characters are fine?

            }
            else
            {
                MessageBox.Show("Enter username");
            }

        }
    }
}
