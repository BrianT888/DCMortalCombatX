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

            // Start a background thread that periodically updates the list of lobbies
            System.Threading.Thread pollThread = new System.Threading.Thread(() =>
            {
                while (keepPolling)
                {
                    List<Lobby> lobbies = foob.GetAllLobbies();

                    // Update UI
                    Dispatcher.Invoke(() =>
                    {
                       LobbyList.ItemsSource = lobbies.Select(lobby => lobby.LobbyName).ToList();     // Get every name from every lobby and make it a list item
                    });

                    System.Threading.Thread.Sleep(1000);     // Wait 1 second before the next list refresh
                }
            });
            pollThread.IsBackground = true;     // Allows thread to automatically stop when UI thread ends
            pollThread.Start();
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

        private void JoinLobbyButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ExitGameButton_Click(object sender, RoutedEventArgs e)
        {

        }

        protected override void OnClosed(EventArgs e)
        {
            keepPolling = false;
        }
    }
}
