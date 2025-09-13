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
        public event Action<string> LobbyMessagesUpdated;
        public event Action<string> LobbyPlayersUpdated;
        public event Action<string> PrivateMessageReceived;


        public void OnLobbyCreated(string lobbyName)
        {
            LobbyCreated?.Invoke(lobbyName);          
        }

        public void OnLobbyMessagesUpdated(string lobbyName)
        {
            LobbyMessagesUpdated?.Invoke(lobbyName);
        }

        public void OnLobbyPlayersUpdated(string lobbyName)
        {
            LobbyPlayersUpdated?.Invoke(lobbyName);
        }

        public void OnPrivateMessageReceived(string targetPlayer)
        {
            PrivateMessageReceived?.Invoke(targetPlayer);
        }
    }
}
