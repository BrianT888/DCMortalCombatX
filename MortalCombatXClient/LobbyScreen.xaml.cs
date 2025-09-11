using GamingLobbyServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MortalCombatXClient
{
    /// <summary>
    /// Interaction logic for LobbyScreen.xaml
    /// </summary>
    public partial class LobbyScreen : Window
    {
        private GamingLobbyServerInterface foob;
        private string Username;
        public LobbyScreen(string username, GamingLobbyServerInterface foob)
        {
            InitializeComponent();
            Username = username;
            this.foob = foob;
        }

        private void CreateLobbyButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void JoinLobbyButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ExitGameButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
