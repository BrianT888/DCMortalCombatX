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
        void OnLobbyMessagesUpdated(string lobbyName);
        [OperationContract(IsOneWay = true)]
        void OnLobbyPlayersUpdated(string lobbyName);

        [OperationContract(IsOneWay = true)]
        void OnPrivateMessageReceived(string targetPlayer);
    }
}
