using MortalCombatXLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace MortalCombatXData
{
    // Interface for all the methods needed to access/modify the stored data.
    // Section for each major part:
    // Player Management - Handles creating/removing players from the system when they log in/out.
    // Lobby Management - Handles creating/deleting lobbies as well as players joining/leaving lobbies.
    // Message Management - Handles messaging within the lobby and private messaging between players.
    // File Sharing - Handles sharing files in the lobby.

    [ServiceContract]
    public interface DataServerInterface
    {
        // --- Player Management --- 
        [OperationContract]
        bool AddNewPlayer(Player player);
        [OperationContract]
        bool RemovePlayer(Player player);
        [OperationContract]
        List<Player> GetAllPlayers();

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

        // --- Message Management ---
        [OperationContract]
        bool LobbyMessage(string lobbyName, Message message);
        [OperationContract]
        List<Message> getLobbyMessages(string lobbyName);
        [OperationContract]
        bool PrivateMessage(string fromUser, string toUser, Message message);

        // --- File Sharing ---
        [OperationContract]
        bool ShareFile(string lobbyName, string link);     // Not sure how to implement the link, prob have to be changed to smth else
        [OperationContract]
        bool GetSharedFiles(string lobbyName);

    }
}
