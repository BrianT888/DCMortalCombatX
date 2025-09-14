using MortalCombatXLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace MortalCombatXData
{
    // Interface for all the methods needed to access/modify the stored data.
    // Section for each major part:
    // Player Management - Handles creating/removing players from the system when they log in/out.
    // Lobby Management - Handles creating/deleting lobbies as well as players joining/leaving lobbies.
    // Message Management - Handles messaging within the lobby and private messaging between players.
    // File Sharing - Handles sharing files in the lobby.

    [ServiceContract]
    public interface DataServerInterface//brian- perhaps change it to MortalCombatXDataInterface?
    {
        // --- Player Management --- 
        [OperationContract]
        bool AddNewPlayer(string username);
        [OperationContract]
        bool RemovePlayer(string username);
        [OperationContract]
        List<Player> GetAllPlayers();
        [OperationContract]
        Player GetPlayer(string username);
        [OperationContract]
        void SetIsLoggedIn(string username, bool isLoggedIn);

        // --- Lobby Management ---
        [OperationContract]
        bool CreateLobby(string lobbyName);
        [OperationContract]
        bool DeleteLobby(string lobbyName);
        [OperationContract]
        bool AddPlayerToLobby(string lobbyName, string username);
        [OperationContract]
        bool RemovePlayerFromLobby(string lobbyName, string username);
        [OperationContract]
        List<Lobby> GetAllLobbies();
        [OperationContract]
        List<Player> GetPlayersInLobby(string lobbyName);

        // --- Message Management ---
        [OperationContract]
        void SendLobbyMessage(string lobbyName, PlayerMessage message);
        [OperationContract]
        List<PlayerMessage> GetLobbyMessages(string lobbyName);
        [OperationContract]
        void SendPrivateDM(string user, string targetPlayer, PlayerMessage message);
        [OperationContract]
        ConcurrentQueue<PlayerMessage> GetDMS(string user, string targetPlayer);
        [OperationContract]
        bool CheckNewDM(string user);
        [OperationContract]
        void SetDMFlag(string user, bool flag);
        [OperationContract]
        string GetLastDMFrom(string target);
    }
}
