using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using MortalCombatXLibrary;
using System.Runtime.InteropServices;
using System.Globalization;

namespace MortalCombatXData
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false)]
    public class DataServer : DataServerInterface
    {
        private static Database db = new Database();

        // --- Player Management ---
        public bool AddNewPlayer(string username)
        {
            return db.AddPlayer(username);
        }

        public bool RemovePlayer(string username)
        {
            // Remove from global player list
            bool removed = db.RemovePlayer(username);

            // Remove player from any lobbies
            foreach (string lobbyName in db.Lobbies.Keys)
            {
                db.RemovePlayerFromLobby(lobbyName, username);
            }

            return removed;
        }

        public List<Player> GetAllPlayers()
        {
            return db.GetAllPlayers();
        }


        // --- Lobby Management ---
        public bool CreateLobby(string lobbyName)
        {
            return db.CreateLobby(lobbyName);
        }

        public bool DeleteLobby(string lobbyName)
        {
            return db.DeleteLobby(lobbyName);
        }

        public bool AddPlayerToLobby(string lobbyName, string username)
        {
            return db.AddPlayerToLobby(lobbyName, username);
        }

        public bool RemovePlayerFromLobby(string lobbyName, string username)
        {
            return db.RemovePlayerFromLobby(lobbyName, username);
        }

        public List<Lobby> GetAllLobbies()
        {
            return db.GetAllLobbies();
        }

        public List<Player> GetPlayersInLobby(string lobbyName)
        {
            return db.GetPlayersInLobby(lobbyName);
        }


        // --- Message Management ---
        public bool SendLobbyMessage(string lobbyName, PlayerMessage message)
        {
            db.SendLobbyMessage(lobbyName, message);
            return true;
        }

        public List<PlayerMessage> GetLobbyMessages(string lobbyName)
        {
            return db.GetLobbyMessages(lobbyName);
        }

        public bool PrivateMessage(string from, string to, PlayerMessage message)     // *** TEMPORARY
        {
            return true;
        }


        // --- File Sharing ---
        public bool ShareFile(string lobbyName, string link)     // *** TEMPORARY
        {
            return true;
        }

        public bool GetSharedFiles(string lobbyName)     // *** TEMPORARY
        {
            return true;
        }
    }
}
