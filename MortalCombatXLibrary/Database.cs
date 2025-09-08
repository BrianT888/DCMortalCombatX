using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
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

        // Send a message in a lobby
        public void SendMessage(string lobbyName, string sender, string content) 
        { 
            if (Lobbies.TryGetValue(lobbyName, out Lobby lobby))
            {
                lobby.Messages.Enqueue(new Message { Sender = sender, Content = content });
            }
        }
        // Get all messages in a lobby
        public List<Message> GetMessages(string lobbyName)
        {
            if (Lobbies.TryGetValue(lobbyName, out Lobby lobby))
            {
                return lobby.Messages.ToList();
            }
            return null;
        }

    }
}
