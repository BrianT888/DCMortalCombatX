using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MortalCombatXLibrary
{
    public interface DuplexScreen
    {
        void OnLobbyCreated(string lobbyName);
        void OnLobbyMessagesUpdated(string lobbyName, List<ChatMessage> messages);
        void OnLobbyPlayersUpdated(string lobbyName, List<Player> players);
        void OnPrivateMessageReceived(string targetPlayer, List<PlayerMessage> messages);
    }
}
