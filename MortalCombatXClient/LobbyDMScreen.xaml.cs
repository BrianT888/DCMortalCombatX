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
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MortalCombatXClient
{
    /// <summary>
    /// Interaction logic for LobbyDMScreen.xaml
    /// </summary>
    public partial class LobbyDMScreen : Window
    {
        private GamingLobbyServerInterface foob;//brian - im just keeping the same style as you did for the previous screen
        private string username;//brian - personally i think the loginscreens string Username should be lower case for camelcase purposes
        private string lobbyName;
        private bool keepPolling = true;
        public LobbyDMScreen(string username, string lobbyName, GamingLobbyServerInterface foob)
        {
            InitializeComponent();
            this.username = username;
            this.lobbyName = lobbyName;
            this.foob = foob;

            LobbyNameTitle.Text = lobbyName;
            Polling();
        }

        private async void PrivateDMPlayerButton_Click(object sender, RoutedEventArgs e)
        {
            if(PlayersInLobbyList.SelectedItem != null)
            {
                string selectedPlayer = PlayersInLobbyList.SelectedItem.ToString();
                if(selectedPlayer == username)
                {
                    MessageBox.Show("You cannot DM yourself!", "Invalid Selection");
                    return;
                }
                PrivateDMScreen privateDMScreen = new PrivateDMScreen(username, selectedPlayer, foob, this);//brian - this ref is for privatedm field to sync closing
                privateDMScreen.Show();
            }
            else
            {
                MessageBox.Show("Please select a player to DM.", "No Player Selected");
            }
        }

        private async void ShareFileButton_Click(object sender, RoutedEventArgs e)
        {
            //brian - gonna stay empty for awhile  
        }

        private async void LogoutButton_Click(object sender, RoutedEventArgs e)//brian - self explanatory
        {
            try
            {
                keepPolling = false;
                await Task.Run(() => foob.Logout(username));
                LoginScreen loginScreen = new LoginScreen();
                loginScreen.Show();
                this.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error logging out: " + ex.Message, "Logout Error");
            }

        }

        private async void ReturnButton_Click(object sender, RoutedEventArgs e)//brian - self explanatory
        {
            try
            {
                keepPolling = false;
                await Task.Run(() => foob.LeaveLobby(lobbyName, username));
                LobbyScreen lobbyScreen = new LobbyScreen(username, foob);
                lobbyScreen.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error leaving lobby: " + ex.Message, "Leave Lobby Error");
            }
        }

        private async void SendMessageInLobbyButton_Click(object sender, RoutedEventArgs e)
        {
            string text = InputMessageTextBox.Text.Trim();
            if (!string.IsNullOrEmpty(username))
            {
                await Task.Run(() => foob.SendLobbyMessage(lobbyName, username, text));
                InputMessageTextBox.Clear();
                await UpdateLobbyStats();
            }
        }

        private async Task UpdateLobbyStats()
        {
            List<PlayerMessage> lobbyMessages = await Task.Run(() => foob.GetLobbyMessages(lobbyName));//brian - doing it same format but i don't think we need an await for any of these?, also randomly just got a communcation fault on GetLobbyMsgs, but i think thats cause smt else died shouldn't happen normally?
            List<string> displayMessages = new List<string>();
            List<Player> playersInLobby = await Task.Run(() => foob.GetPlayersInLobby(lobbyName));
            List<string> displayPlayers = new List<string>();

            foreach (Player p in playersInLobby)
            {
                displayPlayers.Add(p.Username);
            }

            foreach (PlayerMessage msg in lobbyMessages)//brian - for each message we extract sender and content and format it to display
            {
                string text = msg.Sender + ": " + msg.Content;
                displayMessages.Add(text);
            }

            //brian - diff thread currently, use dispatcher to update ui lists
            Dispatcher.Invoke(() => 
            {
                LobbyChatList.ItemsSource = displayMessages; 
                PlayersInLobbyList.ItemsSource = displayPlayers;
                PlayerCounter.Text = "Players in Lobby: " + playersInLobby.Count;
            });
        }

        private async void Polling()//brian - my version of ur polling system
        {
            while (keepPolling)
            {
                await UpdateLobbyStats();
                await Task.Delay(2000);
            }
        }

        protected override void OnClosed(EventArgs e)//brian -  is this necessary? i think its for when you close the window with the x button right? i see it in lobbyscreen.xaml
        {
            keepPolling = false;
        }
    }
}
