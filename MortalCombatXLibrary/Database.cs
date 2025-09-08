using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MortalCombatXLibrary
{
    public class Database
    {
        public List<Player> Players { get; set; } = new List<Player>();     // List of online players
        
        // Dictionary of all lobbies. Key = name of lobby, value = Lobby object
        public Dictionary<string, Lobby> Lobbies { get; set; } = new Dictionary<string, Lobby>();

        // Not sure if we should store messages/message history in here, or store it in each Lobby object (prob more sense to store in each object)

    }
}
