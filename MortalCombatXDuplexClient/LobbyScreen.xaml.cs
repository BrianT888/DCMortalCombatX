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
using System.Diagnostics;

namespace MortalCombatXDuplexClient
{
    /// <summary>
    /// Interaction logic for LobbyScreen.xaml
    /// </summary>
    public partial class LobbyScreen : Window
    {
        private GamingLobbyServerDuplexInterface foob;
        private string Username;
        private CallbackHandler callbackHandler;
        public LobbyScreen(string username, GamingLobbyServerDuplexInterface foob, CallbackHandler callbackHandler)
        {
            InitializeComponent();
            Username = username;
            UsernameTitle.Text = username;
            this.foob = foob;
            this.callbackHandler = callbackHandler;

            // Subscribe to events
            callbackHandler.LobbyCreated += async (lobbyName) => await Dispatcher.InvokeAsync(UpdateLobbyList);
            callbackHandler.LobbyPlayersUpdated += async (lobbyName) => await Dispatcher.InvokeAsync(UpdateLobbyList);

            // Load initial lobby list
            Task.Run(async () => await UpdateLobbyList());           
        }

        // Update UI to show list of lobby names and number of players in the lobby
        public async Task UpdateLobbyList()
        {
            try
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
            catch (Exception ex)
            {
                Dispatcher.Invoke(() => MessageBox.Show("Error updating lobby list: " + ex.Message));
            }
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

                    // Go to LobbyDMScreen
                    LobbyDMScreen lobbyDMScreen = new LobbyDMScreen(Username, lobbyName, foob, callbackHandler);
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
            LogoutButton.IsEnabled = false;//brian - is this necessary? in my one for LobbyDMScreen i don't even both with isEnabled for the button, i swear its instantaneous

            try
            {
                await Task.Run(() => foob.Logout(Username));

                Application.Current.Shutdown();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error logging out: " + ex.Message, "Logout Error");
                LogoutButton.IsEnabled = true;
            }
        }
    }
}
