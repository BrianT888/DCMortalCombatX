using GamingLobbyServer;
using MortalCombatXLibrary;
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
        private GamingLobbyServerInterface foob;
        private string username;
        private string targetPlayer;
        private bool keepPolling = true;
        public PrivateDMScreen(string username, string targetPlayer, GamingLobbyServerInterface foob)
        {
            InitializeComponent();
            this.username = username;
            this.targetPlayer = targetPlayer;
            this.foob = foob;

            TargetPlayer.Text = targetPlayer;

            Polling();
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

        private async Task UpdateDMStats()
        {
            Queue<PlayerMessage> dms = await Task.Run(() => foob.GetDMS(username, targetPlayer));
            List<string> dmDisplayList = new List<string>();

            foreach (PlayerMessage dm in dms)
            {
                string text = dm.Sender.ToString() + ": " + dm.Content;
                dmDisplayList.Add(text);
            }

            Dispatcher.Invoke(() => LobbyChatList.ItemsSource = dmDisplayList);
        }
        private async void Polling()//brian - my version of ur polling system
        {
            while (keepPolling)
            {
                await UpdateDMStats();
                await Task.Delay(2000);
            }
        }
    }
}
