using MortalCombatXLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MortalCombatXDuplexClient
{
    // Callback handler for duplex server
    public class CallbackHandler : GamingLobbyCallback
    {
        // Events that screens can subscribe to
        public event Action<string> LobbyCreated;
        public event Action<string, List<ChatMessage>> LobbyMessagesUpdated;
        public event Action<string, List<Player>> LobbyPlayersUpdated;
        public event Action<string, List<PlayerMessage>> PrivateMessageReceived;

        public void OnLobbyCreated(string lobbyName)
        {
            LobbyCreated?.Invoke(lobbyName);          
        }

        public void OnLobbyMessagesUpdated(string lobbyName, List<ChatMessage> messages)
        {
            LobbyMessagesUpdated?.Invoke(lobbyName, messages);
        }

        public void OnLobbyPlayersUpdated(string lobbyName, List<Player> players)
        {
            LobbyPlayersUpdated?.Invoke(lobbyName, players);
        }

        public void OnPrivateMessageReceived(string targetPlayer, List<PlayerMessage> messages)
        {
            PrivateMessageReceived?.Invoke(targetPlayer, messages);
        }
    }
}
