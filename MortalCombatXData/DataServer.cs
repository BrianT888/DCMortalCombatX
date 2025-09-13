using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using MortalCombatXLibrary;
using System.Runtime.InteropServices;
using System.Globalization;
using System.Collections.Concurrent;

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

        public Player GetPlayer(string username)
        {
            return db.GetPlayer(username);
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
        public void SendLobbyMessage(string lobbyName, PlayerMessage message)
        {
            db.SendLobbyMessage(lobbyName, message);
        }

        public List<PlayerMessage> GetLobbyMessages(string lobbyName)
        {
            return db.GetLobbyMessages(lobbyName);
        }

        public void SendPrivateDM(string user, string targetPlayer, PlayerMessage message)//brian - need quality check
        {
            db.SendPrivateDM(user, targetPlayer, message);
        }

        public ConcurrentQueue<PlayerMessage> GetDMS(string user, string targetPlayer) //brian - this links to the Database.cs GetDMS function i made
        {
            return db.GetDMS(user, targetPlayer);
        }

        public bool CheckNewDM(string user)//brian - added this to check if a user has new dms
        {
            return db.CheckNewDM(user);
        }

        public void SetDMFlag(string user, bool flag)//brian - added this to set the new dm flag for a user
        {
            db.SetDMFlag(user, flag);
        }

        public string GetLastDMFrom(string target)
        {
            return db.GetLastDMFrom(target);
        }
    }
}
