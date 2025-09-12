using GamingLobbyServer;
using MortalCombatXLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Collections.Concurrent;

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
        LobbyDMScreen lobbyDMScreen;//brian - this is to close the lobbydm when we decide to logout from the privatedm 
        //brian - though i believe if we have multiple private dms open i don't think logging out from one will close the others
        //brian - but i think we can do a reverse method, where if this lobbydm is closed, it will close all private dms that are holding this object?


        //brian - ok we just now need like polling to check whether a new dm has been sent, so we can auto open a new window on target client to private dms
        public PrivateDMScreen(string username, string targetPlayer, GamingLobbyServerInterface foob, LobbyDMScreen lobbyDMScreen)
        {
            InitializeComponent();
            this.username = username;
            this.targetPlayer = targetPlayer;
            this.foob = foob;
            this.lobbyDMScreen = lobbyDMScreen;

            TargetPlayer.Text = targetPlayer;

            Polling();
        }
        private async void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                keepPolling = false;
                await Task.Run(() => foob.Logout(username));
                LoginScreen loginScreen = new LoginScreen();
                loginScreen.Show();
                lobbyDMScreen.Close();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error logging out: " + ex.Message, "Logout Error");
            }
        }

        private void ReturnButton_Click(object sender, RoutedEventArgs e)//brian - i think returning from a private dm should just close the window nothing else so no need for async since not using foob?
        {
            try
            {
                keepPolling = false;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error leaving lobby: " + ex.Message, "Leave Lobby Error");
            }
        }

        private async void SendDMButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string text = InputMessageTextBox.Text.Trim();

                if (!string.IsNullOrEmpty(text))
                {
                    await Task.Run(() => foob.SendPrivateDM(username, targetPlayer, text));           
                    Debug.WriteLine("quick check " + foob.GetPlayer(targetPlayer).hasNewDMOrNot);//brian - debug line
                    InputMessageTextBox.Clear();
                    await UpdateDMStats();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error sending DM: " + ex.Message, "Send DM Error");
            }
        }

        private async Task UpdateDMStats()
        {
            ConcurrentQueue<PlayerMessage> dms = await Task.Run(() => foob.GetDMS(username, targetPlayer));//brian - random fault exception happened?
            if(dms != null)//brian - this is to avoid null reference, when dming someone for the first time
            {
                List<string> dmDisplayList = new List<string>();

                foreach (PlayerMessage dm in dms)
                {
                    string text = dm.Sender.ToString() + ": " + dm.Content;
                    dmDisplayList.Add(text);
                }

                Dispatcher.Invoke(() => LobbyChatList.ItemsSource = dmDisplayList);
            }
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
