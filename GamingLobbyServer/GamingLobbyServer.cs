using MortalCombatXLibrary;
using System;
using System.Collections.Generic;
using MortalCombatXData;
using System.ServiceModel;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Diagnostics;
using System.Collections.Concurrent;
using System.Threading;

namespace GamingLobbyServer
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false, InstanceContextMode = InstanceContextMode.Single)]
    public class GamingLobbyServer : GamingLobbyServerInterface
    {
        private DataServerInterface foob;
        uint LogNumber = 0;
        private Dictionary<string, GamingLobbyCallback> duplexClients = new Dictionary<string, GamingLobbyCallback>();

        public GamingLobbyServer()
        {
            foob = GetDataChannel();
        }

        // Duplex specific method
        public void RegisterForCallbacks(string username)
        {
            var callback = OperationContext.Current.GetCallbackChannel<GamingLobbyCallback>();
            if (!duplexClients.ContainsKey(username))
            {
                duplexClients.Add(username, callback);
            }
        }

        // Creates channel to Data Tier
        private DataServerInterface GetDataChannel()
        {   
            ChannelFactory<DataServerInterface> factory = new ChannelFactory<DataServerInterface>(new NetTcpBinding(), "net.tcp://localhost:8100/DataService");
            return factory.CreateChannel();
        }

        // Logs changes to data
        [MethodImpl(MethodImplOptions.Synchronized)]
        private void Log(string message)
        {
            LogNumber++;
            Console.WriteLine("Log " + LogNumber + ": " + message);
        }

        // --- Player Management ---
        public bool Login(string username)
        {
            // Check if username already exists
            List<Player> currentPlayers = foob.GetAllPlayers();
            foreach (Player p in currentPlayers)
            {
                if (p.Username.ToLower() == username.ToLower())     
                {
                    var fault = new UsernameExistsFault { Message = "That username is already taken." };
                    throw new FaultException<UsernameExistsFault>(fault, new FaultReason(fault.Message));
                }
            }

            bool success = foob.AddNewPlayer(username);
            if (success)
            {
                Log($"Login successful for '{username}'.\nPlayer list: " + string.Join(", ", foob.GetAllPlayers().Select(p => p.Username)));
            }
            return success;
        }

        public bool Logout(string username)
        {
            bool success = foob.RemovePlayer(username);
            if (success)
            {
                Log($"Logout successful for '{username}'.\nPlayer list: " + string.Join(", ", foob.GetAllPlayers().Select(p => p.Username)));
            }
            return success;
        }

        public List<Player> GetAllPlayers()
        {
            try
            {
                return foob.GetAllPlayers();
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }

        public Player GetPlayer(string username)
        {
            try
            {
                return foob.GetPlayer(username);
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }


        // --- Lobby Management ---
        public void CreateLobby(string lobbyName)
        {
            // Check if lobby name already exists
            List<Lobby> currentLobbies = foob.GetAllLobbies();
            foreach (Lobby l in currentLobbies)
            {
                if (l.LobbyName.ToLower() == lobbyName.ToLower())
                {
                    var fault = new LobbyExistsFault { Message = "That lobby name already exists." };
                    throw new FaultException<LobbyExistsFault>(fault, new FaultReason(fault.Message));
                }
            }

            if (foob.CreateLobby(lobbyName))
            {
                Log($"Lobby '{lobbyName}' successfully created.\nLobby list: " + string.Join(", ", foob.GetAllLobbies().Select(l => l.LobbyName)));
            }

            // Notify duplex clients
            foreach (var client in duplexClients.Values)
            {
                client.OnLobbyCreated(lobbyName);                
            }
        }

        public bool DeleteLobby(string lobbyName)
        {
            bool success = foob.DeleteLobby(lobbyName);
            if (success)
            {
                Log($"Lobby '{lobbyName}' successfully deleted.\nLobby list: " + string.Join(", ", foob.GetAllLobbies().Select(l => l.LobbyName)));
            }
            return success;
        }

        public bool JoinLobby(string lobbyName, string username)
        {
            bool success = foob.AddPlayerToLobby(lobbyName, username);
            if (success)
            {
                Log($"Player '{username}' successfully joined Lobby '{lobbyName}'.\nPlayer list in '{lobbyName}': " + string.Join(", ", foob.GetPlayersInLobby(lobbyName).Select(p => p.Username)));
            }
            return success;
        }

        public bool LeaveLobby(string lobbyName, string username)
        {
            bool success = foob.RemovePlayerFromLobby(lobbyName, username);
            if (success)
            {
                Log($"Player '{username}' successfully left Lobby '{lobbyName}'.\nPlayer list in '{lobbyName}': " + string.Join(", ", foob.GetPlayersInLobby(lobbyName).Select(p => p.Username)));
            }
            return success;
        }

        public List<Lobby> GetAllLobbies()
        {
            try
            {
                return foob.GetAllLobbies();
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
                return foob.GetPlayersInLobby(lobbyName);
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
                return foob.SendLobbyMessage(lobbyName, new PlayerMessage{ Sender = username, Content = message });
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }

        public bool SendPrivateDM(string user, string targetPlayer, string message)//brian - still need to quality check
        {
            try
            {
                foob.SendPrivateDM(user, targetPlayer, new PlayerMessage { Sender = user, Content = message});
                return true;//brian - lowk idk what to do this this return value lol
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }

        public ConcurrentQueue<PlayerMessage> GetDMS(string user, string targetPlayer)//brian - still need to quality check
        {
            try
            {
                return foob.GetDMS(user, targetPlayer);
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
                return foob.GetLobbyMessages(lobbyName);
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }

        public bool CheckNewDM(string user)//brian - still need to quality check
        {
            try
            {
                return foob.CheckNewDM(user);
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }

        public void SetDMFlag(string user, bool flag)//brian - still need to quality check
        {
            try
            {
                foob.SetDMFlag(user, flag);
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }

        public string GetLastDMFrom(string user)//brian - still need to quality check
        {
            try
            {
                return foob.GetPlayer(user).lastDMFrom;
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
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }
    }
}
