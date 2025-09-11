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
    /// Interaction logic for PrivateDMScreen.xaml
    /// </summary>
    public partial class PrivateDMScreen : Window
    {
        public PrivateDMScreen(string username, string targetPlayer, GamingLobbyServerInterface foob)
        {
            InitializeComponent();
        }
        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ReturnButton_Click(object sender, RoutedEventArgs e)
        {
            // return back to the lobby screen
        }

        private void SendDMButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
