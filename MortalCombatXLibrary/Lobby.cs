using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MortalCombatXLibrary
{
    public class Lobby
    {
        public String lobbyName { get; set; }
        public List<Player> players { get; set; }//list of players in the lobby currently?
        //do i need like a list of strings for the text msgs?
        public Lobby(String lobbyName)
        {
            this.lobbyName = lobbyName;
            players = new List<Player>();
        }

        //perhaps a method to add players to the lobby?
        public void AddPlayer(Player player)//might be useful maybe
        {
            players.Add(player);
        }
        //method to for players to leave the lobby
        public void RemovePlayer(Player player)
        {
            players.Remove(player);
        }
        //return list of players to be used to show which players are currently in the lobby?
        public List<Player> showPlayers()
        {
            return players;
        }
    }
}
