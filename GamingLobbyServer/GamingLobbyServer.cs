using MortalCombatXLibrary;
using System;
using System.Collections.Generic;
using MortalCombatXData;
using System.ServiceModel;

namespace GamingLobbyServer
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false)]
    public class GamingLobbyServer : GamingLobbyServerInterface
    {
        uint LogNumber = 0; //brian - just incase we wanted to do the log thing and keep track of stuff imma keep this here, if not we can remove it later

        // Creates channel to Data Tier
        private DataServerInterface GetDataChannel()
        {
            ChannelFactory<DataServerInterface> factory = new ChannelFactory<DataServerInterface>(new NetTcpBinding(), "net.tcp://localhost:8100/DataService");

            return factory.CreateChannel();
        }

        // --- Player Management ---
        public bool Login(string username)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(username) || username.Length > 25)     // Username can't be empty or longer than 25 characters
                {
                    return false;
                }

                DataServerInterface channel = GetDataChannel();

                // Check if username already exists
                List<Player> currentPlayers = channel.GetAllPlayers();
                foreach (Player p in currentPlayers)
                {
                    if (p.Username.ToLower() == username.ToLower())
                    {
                        return false;     // Will change this to a custom exception that contains an infomartive message about the error
                    }
                }

                return channel.AddNewPlayer(username);
            }
            catch (Exception ex)
            {
                throw new Exception();  //temp
            }
        }

        public bool Logout(string username)
        {

            try
            {
                DataServerInterface channel = GetDataChannel();
                return channel.RemovePlayer(username);
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }

        public List<Player> GetAllPlayers()
        {
            try
            {
                DataServerInterface channel = GetDataChannel();
                return channel.GetAllPlayers();
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }


        // --- Lobby Management ---
        public bool CreateLobby(string lobbyName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(lobbyName) || lobbyName.Length > 25)     // Lobby name can't be empty or longer than 25 characters
                {
                    return false;
                }

                DataServerInterface channel = GetDataChannel();

                // Check if lobby name already exists
                List<Lobby> currentLobbies = channel.GetAllLobbies();
                foreach (Lobby l in currentLobbies)
                {
                    if (l.LobbyName.ToLower() == lobbyName.ToLower())
                    {
                        return false;     // Will change this to a custom exception that contains an infomartive message about the error
                    }
                }

                return channel.CreateLobby(lobbyName);
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }

        public bool DeleteLobby(string lobbyName)
        {
            try
            {
                DataServerInterface channel = GetDataChannel();
                return channel.DeleteLobby(lobbyName);
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }

        public bool JoinLobby(string lobbyName, string username)
        {
            try
            {
                DataServerInterface channel = GetDataChannel();
                return channel.AddPlayerToLobby(lobbyName, username);
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }

        public bool LeaveLobby(string lobbyName, string username)
        {
            try
            {
                DataServerInterface channel = GetDataChannel();
                return channel.RemovePlayerFromLobby(lobbyName, username);
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }

        public List<Lobby> GetAllLobbies()
        {
            try
            {
                DataServerInterface channel = GetDataChannel();
                return channel.GetAllLobbies();
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }

        public List<Player> GetPlayersInLobby(string lobbyName)
        {
            try
            {
                DataServerInterface channel = GetDataChannel();
                return channel.GetPlayersInLobby(lobbyName);
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }


        // --- Message Management ---
        public bool SendLobbyMessage(string lobbyName, string username, string message)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(message) || message.Length > 100)     // Message can't be empty or longer than 100 characters
                {
                    return false;
                }

                DataServerInterface channel = GetDataChannel();
                return channel.SendLobbyMessage(lobbyName, new PlayerMessage{ Sender = username, Content = message });
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }

        public bool SendPrivateMessage(string fromUser, string toUser, string message)     // not done
        {
            try
            {
                DataServerInterface channel = GetDataChannel();
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }

        public List<PlayerMessage> GetLobbyMessages(string lobbyName)
        {
            try
            {
                DataServerInterface channel = GetDataChannel();
                return channel.GetLobbyMessages(lobbyName);
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }


        // --- File Sharing ---
        public bool GetSharedFiles(string lobbyName)     // not done
        {
            try
            {
                DataServerInterface channel = GetDataChannel();
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }



        public bool ShareFile(string lobbyName, string link)     // not done
        {
            try
            {
                DataServerInterface channel = GetDataChannel();
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }
    }
}
