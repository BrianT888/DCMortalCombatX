using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MortalCombatXLibrary
{
    public class Lobby
    {
        public String LobbyName { get; set; }
        public List<Player> Players { get; set; } = new List<Player>(); //list of players in the lobby currently?

        //do i need like a list of strings for the text msgs?
        public Lobby(String lobbyName)
        {
            LobbyName = lobbyName;
        }
    }
}
