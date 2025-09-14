using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MortalCombatXLibrary
{
    public class Lobby
    {
        public String LobbyName { get; set; }
        public ConcurrentDictionary<string, Player> Players { get; } = new ConcurrentDictionary<string, Player>(); //list of players in the lobby currently
        // Jaxon: changed it to concurrent dict bc might cause issues if multiple clients access it at the same time
        public ConcurrentQueue<PlayerMessage> Messages { get; } = new ConcurrentQueue<PlayerMessage>();     // Stores all lobby messages (Queue to make sure its in order bc apparently ConcurrentLists dont always stay in order)
    }
}
