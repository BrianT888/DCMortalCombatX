using MortalCombatXLibrary;
using System;
using System.Collections.Generic;
using MortalCombatXData;
using System.ServiceModel;
using System.Runtime.CompilerServices;
using System.Linq;

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

        [MethodImpl(MethodImplOptions.Synchronized)]
        private void Log(string message)
        {
            LogNumber++;
            Console.WriteLine("Log " + LogNumber + ": " + message);
        }

        // --- Player Management ---
        public bool Login(string username)
        {
            DataServerInterface channel = GetDataChannel();

            // Check if username already exists
            List<Player> currentPlayers = channel.GetAllPlayers();
            foreach (Player p in currentPlayers)
            {
                if (p.Username.ToLower() == username.ToLower())     
                {
                    var fault = new UsernameExistsFault { Message = "That username is already taken." };
                    throw new FaultException<UsernameExistsFault>(fault, new FaultReason(fault.Message));
                }
            }

            bool success = channel.AddNewPlayer(username);
            if (success)
            {
                Log($"Login successful for '{username}'.\nPlayer list: " + string.Join(", ", channel.GetAllPlayers().Select(p => p.Username)));
            }
            return success;
        }

        public bool Logout(string username)
        {
            DataServerInterface channel = GetDataChannel();
            bool success = channel.RemovePlayer(username);
            if (success)
            {
                Log($"Logout successful for '{username}'.\nPlayer list: " + string.Join(", ", channel.GetAllPlayers().Select(p => p.Username)));
            }
            return success;
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
            DataServerInterface channel = GetDataChannel();

            // Check if lobby name already exists
            List<Lobby> currentLobbies = channel.GetAllLobbies();
            foreach (Lobby l in currentLobbies)
            {
                if (l.LobbyName.ToLower() == lobbyName.ToLower())
                {
                    var fault = new LobbyExistsFault { Message = "That lobby name already exists." };
                    throw new FaultException<LobbyExistsFault>(fault, new FaultReason(fault.Message));
                }
            }

            bool success = channel.CreateLobby(lobbyName);
            if (success)
            {
                Log($"Lobby '{lobbyName}' successfully created.\nLobby list: " + string.Join(", ", channel.GetAllLobbies().Select(l => l.LobbyName)));
            }
            return success;
        }

        public bool DeleteLobby(string lobbyName)
        {
            DataServerInterface channel = GetDataChannel();
            bool success = channel.DeleteLobby(lobbyName);
            if (success)
            {
                Log($"Lobby '{lobbyName}' successfully deleted.\nLobby list: " + string.Join(", ", channel.GetAllLobbies().Select(l => l.LobbyName)));
            }
            return success;
        }

        public bool JoinLobby(string lobbyName, string username)
        {
            DataServerInterface channel = GetDataChannel();
            bool success = channel.AddPlayerToLobby(lobbyName, username);
            if (success)
            {
                Log($"Player '{username}' successfully joined Lobby '{lobbyName}'.\nPlayer list in '{lobbyName}': " + string.Join(", ", channel.GetPlayersInLobby(lobbyName).Select(p => p.Username)));
            }
            return success;
        }

        public bool LeaveLobby(string lobbyName, string username)
        {
            DataServerInterface channel = GetDataChannel();
            bool success = channel.RemovePlayerFromLobby(lobbyName, username);
            if (success)
            {
                Log($"Player '{username}' successfully left Lobby '{lobbyName}'.\nPlayer list in '{lobbyName}': " + string.Join(", ", channel.GetPlayersInLobby(lobbyName).Select(p => p.Username)));
            }
            return success;
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
