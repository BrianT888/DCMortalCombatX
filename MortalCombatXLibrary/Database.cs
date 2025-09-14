using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MortalCombatXLibrary
{
    public class Database
    {
        // Dict of all players. Key = username, value = Player object
        public ConcurrentDictionary<string, Player> Players { get; } = new ConcurrentDictionary<string, Player>();

        // Dict of all lobbies. Key = name of lobby, value = Lobby object
        public ConcurrentDictionary<string, Lobby> Lobbies { get; } = new ConcurrentDictionary<string, Lobby>();


        // Add a player
        public bool AddPlayer(string username)
        {
            return Players.TryAdd(username, new Player { Username = username });
        }
        // Remove a player
        public bool RemovePlayer(string username)
        {
            return Players.TryRemove(username, out _);
        }
        // Return list of all players
        public List<Player> GetAllPlayers()
        {
            return Players.Values.ToList();
        }

        // Create a lobby
        public bool CreateLobby(string lobbyName)
        {
            return Lobbies.TryAdd(lobbyName, new Lobby { LobbyName = lobbyName });
        }
        // Delete a lobby
        public bool DeleteLobby(string lobbyName)
        {
            return Lobbies.TryRemove(lobbyName, out _);
        }
        // Add a player to a lobby
        public bool AddPlayerToLobby(string lobbyName, string username)
        {
            if (Lobbies.TryGetValue(lobbyName, out Lobby lobby) && Players.TryGetValue(username, out Player player))
            {
                return lobby.Players.TryAdd(username, player);
            }
            return false;
        }
        // Remove a player from a lobby
        public bool RemovePlayerFromLobby(string lobbyName, string username)
        {
            if (Lobbies.TryGetValue(lobbyName, out Lobby lobby))
            {
                return lobby.Players.TryRemove(username, out _);
            }
            return false;
        }
        // Return list of all lobbies
        public List<Lobby> GetAllLobbies()
        {
            return Lobbies.Values.ToList();
        }
        // Return list of players in given lobby
        public List<Player> GetPlayersInLobby(string lobbyName)
        {
            if (Lobbies.TryGetValue(lobbyName, out Lobby lobby))
            {
                return lobby.Players.Values.ToList();
            }
            return null;
        }

        // Send a message in a lobby
        public void SendLobbyMessage(string lobbyName, PlayerMessage message) 
        { 
            if (Lobbies.TryGetValue(lobbyName, out Lobby lobby))
            {
                lobby.Messages.Enqueue(message);
            }
        }
        // Get all messages in a lobby
        public List<PlayerMessage> GetLobbyMessages(string lobbyName)
        {
            if (Lobbies.TryGetValue(lobbyName, out Lobby lobby))
            {
                return lobby.Messages.ToList();
            }
            return null;
        }

        public ConcurrentQueue<PlayerMessage> GetDMS(string user, string targetPlayer)//brian - added this to get dms of a player, returning queue instead of list since i swear we don't need to .ToList() everything like the above no?
        {
            if (Players.TryGetValue(user, out Player player))
            {
                if (player.dms.TryGetValue(targetPlayer, out ConcurrentQueue<PlayerMessage> messages))//brian value is null, parameter is key?
                {
                    return messages;
                }
            }
            return null;
        }

        public void SendPrivateDM(string user, string targetPlayer, PlayerMessage dm)
        {
            if (Players.TryGetValue(user, out Player player) && (Players.TryGetValue(targetPlayer, out Player target)))
            {
                if (player.dms.TryGetValue(targetPlayer, out ConcurrentQueue<PlayerMessage> messages))
                {
                    Debug.WriteLine(user + ": Found existing dm history for " + targetPlayer + " msg is " + dm.Content);
                    messages.Enqueue(dm);
                }
                else//brian - if we haven't dm'd this player before, create new msg history for them
                {
                    Debug.WriteLine(user + ": Creating new dm history for " + targetPlayer + " msg is " + dm.Content);
                    ConcurrentQueue<PlayerMessage> newQueue = new ConcurrentQueue<PlayerMessage>();
                    newQueue.Enqueue(dm);
                    player.dms.TryAdd(targetPlayer, newQueue);
                }

                if (target.dms.TryGetValue(user, out ConcurrentQueue<PlayerMessage> tmessages))
                {
                    Debug.WriteLine(targetPlayer + ": Found existing dm history for " + user + " msg is " + dm.Content);
                    tmessages.Enqueue(dm);
                }
                else//brian - if we haven't dm'd this player before, create new msg history for them
                {
                    Debug.WriteLine(targetPlayer + ": Creating new dm history for " + user + " msg is " + dm.Content);
                    ConcurrentQueue<PlayerMessage> newQueue = new ConcurrentQueue<PlayerMessage>();
                    newQueue.Enqueue(dm);
                    target.dms.TryAdd(user, newQueue);
                }

                GetPlayer(targetPlayer).lastDMFrom = user;//brian - set the target players last dm from to the user who dm'd them
                SetDMFlag(targetPlayer, true);//brian - set the target players new dm flag to true
            }
        }

        public bool CheckNewDM(string target)
        {
            Player player = GetPlayer(target);
            bool hasNewDM = player.hasNewDMOrNot;
            player.hasNewDMOrNot = false;
            return hasNewDM;//brian - get back the player objects bool flag
        }

        public string GetLastDMFrom(string target)
        {
            Player player = GetPlayer(target);
            if (player != null)
            {
                string lastdm = player.lastDMFrom;
                player.lastDMFrom = "";//brian - reset last dm from after we get it
                return lastdm;
            }
            return null;
        }
        public void SetDMFlag(string target, bool flag)//brian - set the flag to false after we open the new dm window
        {
            Player player = GetPlayer(target);
            if (player != null)
            {
                player.hasNewDMOrNot = flag;
            }
        }

        public Player GetPlayer(string username)
        {
            if (Players.TryGetValue(username, out Player player))
            {
                return player;
            }
            return null;
        }
    }
}
