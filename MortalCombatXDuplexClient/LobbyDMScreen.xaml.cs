using GamingLobbyServer;
using Microsoft.Win32;
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
using System.IO;
using System.IO.IsolatedStorage;

namespace MortalCombatXDuplexClient
{
    /// <summary>
    /// Interaction logic for LobbyDMScreen.xaml
    /// </summary>
    public partial class LobbyDMScreen : Window
    {
        private GamingLobbyServerDuplexInterface foob;//brian - im just keeping the same style as you did for the previous screen
        private string username;//brian - personally i think the loginscreens string Username should be lower case for camelcase purposes
        private string lobbyName;
        private bool keepPolling = true;
        private List<PrivateDMScreen> openPrivateDMScreens = new List<PrivateDMScreen>();//brian - list of currently open private dm screens, so we can close them all on logout
        private CallbackHandler callbackHandler;

        public LobbyDMScreen(string username, string lobbyName, GamingLobbyServerDuplexInterface foob, CallbackHandler callbackHandler)
        {
            InitializeComponent();
            this.username = username;
            this.lobbyName = lobbyName;
            this.foob = foob;
            this.callbackHandler = callbackHandler;

            UsernameTitle.Text = username;
            LobbyNameTitle.Text = lobbyName;

            // Subscribe to events
            callbackHandler.LobbyMessagesUpdated += async (lobby) => await Dispatcher.InvokeAsync(UpdateLobbyStats);
            callbackHandler.PrivateMessageReceived += async (targetPlayer) => await Dispatcher.InvokeAsync(PrivateDMWindowPopUp);
            callbackHandler.LobbyPlayersUpdated += async (lobby) => await Dispatcher.InvokeAsync(UpdateLobbyStats);

            // Load initial lobby stats
            Task.Run(async () => await UpdateLobbyStats());
        }

        private async void PrivateDMPlayerButton_Click(object sender, RoutedEventArgs e)
        {
            if (PlayersInLobbyList.SelectedItem != null)
            {
                string selectedPlayer = PlayersInLobbyList.SelectedItem.ToString();
                if (selectedPlayer == username)
                {
                    MessageBox.Show("You cannot DM yourself!", "Invalid Selection");
                    return;
                }

                if (openPrivateDMScreens.Find(pDMScreen => pDMScreen.targetPlayer == selectedPlayer) == null)
                {
                    PrivateDMScreen privateDMScreen = new PrivateDMScreen(username, selectedPlayer, foob, this, callbackHandler);
                    privateDMScreen.Show();
                    openPrivateDMScreens.Add(privateDMScreen);
                }
            }
            else
            {
                MessageBox.Show("Please select a player to DM.", "No Player Selected");
            }
        }

        private async void ShareFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Allowed files|*.txt;*.png;*.jpg";

            if (openFileDialog.ShowDialog() == true)
            {
                string selectedFile = openFileDialog.FileName;
                string fileName = System.IO.Path.GetFileName(selectedFile);
                byte[] fileBytes = File.ReadAllBytes(selectedFile);
                string fileData = Convert.ToBase64String(fileBytes);
                string fileExt = System.IO.Path.GetExtension(selectedFile); // includes the dot, e.g., .txt, .png

                if (!string.IsNullOrEmpty(username))
                {
                    await Task.Run(() => foob.SendLobbyMessage(lobbyName, username, $"FILE|{fileName}|{fileExt}|{fileData}"));
                    await UpdateLobbyStats();
                }
            }
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
            catch (Exception ex)
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
                LobbyScreen lobbyScreen = new LobbyScreen(username, foob, callbackHandler);
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
            List<PlayerMessage> lobbyMessages = await Task.Run(() => foob.GetLobbyMessages(lobbyName));//brian - doing it same format but i don't think we need an await for any of these?
            List<ChatMessage> displayMessagesWithFiles = new List<ChatMessage>();
            List<Player> playersInLobby = await Task.Run(() => foob.GetPlayersInLobby(lobbyName));
            List<string> displayPlayers = new List<string>();

            foreach (Player p in playersInLobby)
            {
                displayPlayers.Add(p.Username);
            }

            foreach (PlayerMessage msg in lobbyMessages)//brian - for each message we extract sender and content and format it to display
            {
                // toma = im gonna make it so that it checks if its a shared FILE message

                if (msg.Content.StartsWith("FILE|"))
                {
                    string[] parts = msg.Content.Split('|');
                    if (parts.Length == 4) // FILE|name|.jpg|base64data
                    {
                        string fileName = parts[1];
                        string fileExt = parts[2];
                        string fileData = parts[3];

                        displayMessagesWithFiles.Add(new ChatMessage
                        {
                            DisplayText = $"{msg.Sender}: Shared file:",
                            IsFile = true, // for the button visibility
                            FileName = fileName,
                            FileExtension = fileExt,
                            FileData = fileData
                        });
                    }
                }
                else
                {
                    // Regular message
                    displayMessagesWithFiles.Add(new ChatMessage
                    {
                        DisplayText = $"{msg.Sender}: {msg.Content}",
                        IsFile = false
                    });
                }

            }

            //brian - diff thread currently, use dispatcher to update ui lists
            Dispatcher.Invoke(() =>
            {
                LobbyChatList.ItemsSource = displayMessagesWithFiles;
                PlayersInLobbyList.ItemsSource = displayPlayers;
                PlayerCounter.Text = "Players in Lobby: " + playersInLobby.Count;
            });
        }

        private async Task PrivateDMWindowPopUp()
        {
            bool newDMorNah = await Task.Run(() => foob.CheckNewDM(username));
            string dmer = await Task.Run(() => foob.GetLastDMFrom(username));

            if (newDMorNah && dmer != null)
            {
                //foob.SetDMFlag(username, false);
                if (openPrivateDMScreens.Find(pDMScreen => pDMScreen.targetPlayer == dmer) == null)
                {
                    PrivateDMScreen privateDMScreen = new PrivateDMScreen(username, dmer, foob, this, callbackHandler);
                    privateDMScreen.Show();
                    openPrivateDMScreens.Add(privateDMScreen);
                }
            }
        }

        private void ViewFileButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            ChatMessage chatMessage = button.Tag as ChatMessage;

            if (chatMessage != null && chatMessage.IsFile)
            {
                OpenFileViewer(chatMessage.FileName, chatMessage.FileExtension, chatMessage.FileData);
            }
        }

        private void OpenFileViewer(string fileName, string fileExt, string fileData)
        {
            Window viewerWindow = new Window();
            viewerWindow.Title = fileName;
            viewerWindow.Width = 500;
            viewerWindow.Height = 500;

            try
            {
                byte[] bytes = Convert.FromBase64String(fileData);

                if (fileExt == ".txt")
                {
                    // Text file viewer
                    TextBox textBox = new TextBox();
                    textBox.Text = Encoding.UTF8.GetString(bytes);
                    textBox.IsReadOnly = true;
                    textBox.TextWrapping = TextWrapping.Wrap;
                    textBox.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
                    viewerWindow.Content = textBox;
                }
                else if (fileExt == ".jpg" || fileExt == ".png")
                {
                    // Image file viewer
                    Image image = new Image();
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit(); // tells wpf its going to initialise since BitMapInage freezes
                    bitmap.StreamSource = new MemoryStream(bytes);
                    bitmap.EndInit();
                    image.Source = bitmap;
                    image.Stretch = Stretch.Uniform; // This keeps aspect ratio
                    viewerWindow.Content = image;
                }
                else
                {
                    MessageBox.Show($"Unsupported file type: {fileExt}");
                    return;
                }

                viewerWindow.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening file: {ex.Message}");
            }
        }

        private async void Polling()//brian - my version of ur polling system
        {
            while (keepPolling)
            {
                await PrivateDMWindowPopUp();
                await UpdateLobbyStats();
                await Task.Delay(2000);
            }
        }

        protected override void OnClosed(EventArgs e)//brian -  is this necessary? i think its for when you close the window with the x button right? i see it in lobbyscreen.xaml
        {
            keepPolling = false;
        }

        public void RemoveClosedPrivateDMScreen(PrivateDMScreen closedScreen)
        {
            openPrivateDMScreens.Remove(closedScreen);
        }
    }
}
