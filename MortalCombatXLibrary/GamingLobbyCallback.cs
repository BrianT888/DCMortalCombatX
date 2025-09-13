using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace MortalCombatXLibrary
{
    [ServiceContract]
    public interface GamingLobbyCallback
    {
        [OperationContract(IsOneWay = true)]
        void OnLobbyCreated(string lobbyName);

        [OperationContract(IsOneWay = true)]
        void OnLobbyMessagesUpdated(string lobbyName, List<ChatMessage> messages);
        [OperationContract(IsOneWay = true)]
        void OnLobbyPlayersUpdated(string lobbyName, List<Player> players);

        [OperationContract(IsOneWay = true)]
        void OnPrivateMessageReceived(string targetPlayer, List<PlayerMessage> message);
    }
}
