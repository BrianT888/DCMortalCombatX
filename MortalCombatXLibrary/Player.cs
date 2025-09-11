using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MortalCombatXLibrary
{
    public class Player
    {
        public string Username { get; set; }
        
        public Dictionary<string, Queue<PlayerMessage>> dms { get; } = new Dictionary<string, Queue<PlayerMessage>>();//brian - i added this for dms, using the string of the target player, we can get the queue of the dms?
    }
}
