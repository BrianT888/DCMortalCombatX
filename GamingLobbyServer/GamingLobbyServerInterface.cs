using MortalCombatXLibrary;
using System.Collections.Generic;
using System.ServiceModel;

namespace GamingLobbyServer
{
    [ServiceContract]
    public interface GamingLobbyServerInterface//practically our business tier
    {
        //Brian - I added a reference to the library, but im unsure if the business interface should ref the library or not?
        //if not some of these methods i pasted from the data int will have to be changed into smt else, then remove the reference
        //mainly the ones involving the player, lobby and player messages?

        // --- Player Management --- 
        [OperationContract]
        [FaultContract(typeof(UsernameExistsFault))]
        bool Login(string username);

        [OperationContract]
        bool Logout(string username);

        [OperationContract]
        List<Player> GetAllPlayers();


        // --- Lobby Management ---
        [OperationContract]
        [FaultContract(typeof(LobbyExistsFault))]
        bool CreateLobby(string lobbyName);

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
        bool SendLobbyMessage(string lobbyName, string username, string message);

        [OperationContract]
        bool SendPrivateDM(string user, string targetPlayer, string message);
        [OperationContract]
        Queue<PlayerMessage> GetDMS(string user, string targetPlayer);

        [OperationContract]
        List<PlayerMessage> GetLobbyMessages(string lobbyName);


        // --- File Sharing ---
        [OperationContract]
        bool ShareFile(string lobbyName, string link);     // Not sure how to implement the link, prob have to be changed to smth else
        [OperationContract]
        bool GetSharedFiles(string lobbyName);
    }
}
