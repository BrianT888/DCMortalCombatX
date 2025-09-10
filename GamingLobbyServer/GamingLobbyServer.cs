using MortalCombatXLibrary;
using System;
using System.Collections.Generic;
using MortalCombatXData;
using System.ServiceModel;

namespace GamingLobbyServer
{
    internal class GamingLobbyServer : GamingLobbyServerInterface
    {
        //brian - haven't done the logic yet
        private DataServerInterface foob;
        uint LogNumber = 0; //brian - just incase we wanted to do the log thing and keep track of stuff imma keep this here, if not we can remove it later

        public GamingLobbyServer()
        {
            //I think we need a constructor for this similar to business?
            ChannelFactory<DataServerInterface> foobFactory;
            NetTcpBinding tcp = new NetTcpBinding();

            string url = "net.tcp://localhost:8100/DataServer";
            foobFactory = new ChannelFactory<DataServerInterface>(tcp, url);
            foob = foobFactory.CreateChannel();
        }


        // --- Player Management ---
        public bool AddNewPlayer(string username)
        {
            throw new NotImplementedException();
        }

        public bool AddPlayerToLobby(string lobbyName, string username)
        {
            throw new NotImplementedException();
        }


        public bool CreateLobby(string lobbyName)
        {
            throw new NotImplementedException();
        }

        public bool DeleteLobby(string lobbyName)
        {
            throw new NotImplementedException();
        }

        public List<Lobby> GetAllLobbies()
        {
            throw new NotImplementedException();
        }

        public List<Player> GetAllPlayers()
        {
            throw new NotImplementedException();
        }

        public List<PlayerMessage> GetLobbyMessages(string lobbyName)
        {
            throw new NotImplementedException();
        }

        public List<Player> GetPlayersInLobby(string lobbyName)
        {
            throw new NotImplementedException();
        }

        public bool GetSharedFiles(string lobbyName)
        {
            throw new NotImplementedException();
        }

        public bool PrivateMessage(string fromUser, string toUser, PlayerMessage message)
        {
            throw new NotImplementedException();
        }

        public bool RemovePlayer(string username)
        {
            throw new NotImplementedException();
        }

        public bool RemovePlayerFromLobby(string lobbyName, string username)
        {
            throw new NotImplementedException();
        }

        public bool SendLobbyMessage(string lobbyName, PlayerMessage message)
        {
            throw new NotImplementedException();
        }

        public bool ShareFile(string lobbyName, string link)
        {
            throw new NotImplementedException();
        }
    }
}
