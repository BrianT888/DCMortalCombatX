using GamingLobbyServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
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
using MortalCombatXLibrary;
using System.Runtime.CompilerServices;

namespace MortalCombatXClient
{
    /// <summary>
    /// Interaction logic for LobbyScreen.xaml
    /// </summary>
    public partial class LobbyScreen : Window
    {
        private GamingLobbyServerInterface foob;
        private string Username;
        private bool keepPolling = true;
        public LobbyScreen(string username, GamingLobbyServerInterface foob)
        {
            InitializeComponent();
            Username = username;
            this.foob = foob;
            
            UsernameTitle.Text = username;

            // Start a background task that periodically updates the list of lobbies
            Task.Run(async () =>
            {
                while (keepPolling)
                {
                    await UpdateLobbyList();
                    await Task.Delay(2000);     // Wait 2 seconds before the next list refresh
                }
            });
        }

        // Update UI to show list of lobby names and number of players in the lobby
        private async Task UpdateLobbyList()
        {
            List<Lobby> lobbies = await Task.Run(() => foob.GetAllLobbies());
            List<string> lobbyDisplayList = new List<string>();

            foreach (var lobby in lobbies)
            {
                try
                {
                    List<Player> players = await Task.Run(() => foob.GetPlayersInLobby(lobby.LobbyName));
                    lobbyDisplayList.Add($"| {lobby.LobbyName} | {players.Count} players |");
                }
                catch
                {
                    lobbyDisplayList.Add($"| {lobby.LobbyName} | 0 players | ");
                }
            }
            Dispatcher.Invoke(() => LobbyList.ItemsSource = lobbyDisplayList);     // Display contents of lobbyDisplayList on the UI
        }

        private async void CreateLobbyButton_Click(object sender, RoutedEventArgs e)
        {
            string lobbyName = CreatedLobbyName.Text.Trim();

            if (!string.IsNullOrEmpty(lobbyName) )
            {
                try
                {
                    CreateLobbyButton.IsEnabled = false;
                    LoadingBar.Visibility = Visibility.Visible;

                    await Task.Run(() => foob.CreateLobby(lobbyName));
                    await UpdateLobbyList();
                    CreatedLobbyName.Text = "";
                }
                catch(FaultException<LobbyExistsFault> ex)
                {
                    LoadingBar.Visibility = Visibility.Collapsed;
                    MessageBox.Show(ex.Detail.Message, "Lobby Creation Error");
                }
                finally
                {
                    LoadingBar.Visibility = Visibility.Collapsed;
                    CreateLobbyButton.IsEnabled = true;
                }
            }
            else
            {
                MessageBox.Show("Enter the lobby name");
            }
        }

        // Puts user into selected lobby
        private async void JoinLobbyButton_Click(object sender, RoutedEventArgs e)
        {
            if (LobbyList.SelectedItem != null)
            {
                try
                {
                    // Get the lobby name from the selected string in the UI ListBox (format = "| LobbyName | X players |")
                    string selectedText = LobbyList.SelectedItem.ToString();
                    string lobbyName = selectedText.Split('|')[1].Trim();

                    await Task.Run(() => foob.JoinLobby(lobbyName, Username));
                    keepPolling = false;

                    // Go to LobbyDMScreen
                    LobbyDMScreen lobbyDMScreen = new LobbyDMScreen(Username, lobbyName, foob);
                    lobbyDMScreen.Show();
                    this.Close();
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Error joining lobby: " + ex.Message);   
                }
            }
            else
            {
                MessageBox.Show("Please select a lobby to join.");
            }
        }

        private async void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            LogoutButton.IsEnabled = false;

            try
            {
                keepPolling = false;

                await Task.Run(() => foob.Logout(Username));
                LoginScreen loginScreen = new LoginScreen();
                loginScreen.Show();
                this.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error logging out: " + ex.Message, "Logout Error");
                LogoutButton.IsEnabled = true;
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            keepPolling = false;
        }
    }
}
