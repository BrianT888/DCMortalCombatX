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
        void OnLobbyMessagesUpdated(string lobbyName);
        void OnLobbyPlayersUpdated(string lobbyName);
        void OnPrivateMessageReceived(string targetPlayer);
    }
}
