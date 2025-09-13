using MortalCombatXLibrary;
using System.Collections.Generic;
using System.ServiceModel;
using System.Collections.Concurrent;

namespace GamingLobbyServer
{
    [ServiceContract(CallbackContract = typeof(GamingLobbyCallback))]
    public interface GamingLobbyServerDuplexInterface
    {
        [OperationContract]
        void RegisterForCallbacks(string username);

        // --- Player Management --- 
        [OperationContract]
        [FaultContract(typeof(UsernameExistsFault))]
        bool Login(string username);

        [OperationContract]
        bool Logout(string username);

        [OperationContract]
        List<Player> GetAllPlayers();
        [OperationContract]
        Player GetPlayer(string username);

        // --- Lobby Management ---
        [OperationContract]
        [FaultContract(typeof(LobbyExistsFault))]
        void CreateLobby(string lobbyName);

        [OperationContract]
        bool DeleteLobby(string lobbyName);

        [OperationContract]
        bool JoinLobby(string lobbyName, string username);

        [OperationContract]
        bool LeaveLobby(string lobbyName, string username);

        [OperationContract]
        List<Lobby> GetAllLobbies();

        [OperationContract]
        List<Player> GetPlayersInLobby(string lobbyName);


        // --- Message Management ---
        [OperationContract]
        void SendLobbyMessage(string lobbyName, string username, string message);

        [OperationContract]
        void SendPrivateDM(string user, string targetPlayer, string message);
        [OperationContract]
        ConcurrentQueue<PlayerMessage> GetDMS(string user, string targetPlayer);

        [OperationContract]
        List<PlayerMessage> GetLobbyMessages(string lobbyName);
        [OperationContract]
        bool CheckNewDM(string user);
        [OperationContract]
        void SetDMFlag(string user, bool flag);
        [OperationContract]
        string GetLastDMFrom(string target);
    }
}
