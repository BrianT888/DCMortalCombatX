using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace MortalCombatXLibrary
{
    public class Player
    {
        public string Username { get; set; }
        public bool hasNewDMOrNot { get; set; } = false;//brian - added this to check if we have a new dm
        public ConcurrentDictionary<string, ConcurrentQueue<PlayerMessage>> dms { get; } = new ConcurrentDictionary<string, ConcurrentQueue<PlayerMessage>>();//brian - i added this for dms, using the string of the target player, we can get the queue of the dms?
    }
}
