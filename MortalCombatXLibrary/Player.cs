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
        public bool isLoggedIn {  get; set; }
        public string Username { get; set; }
        public bool hasNewDMOrNot { get; set; } = false;//brian - added this to check if we have a new dm
        public string lastDMFrom { get; set; } = "";//brian - added this to store the last person who dm'd us, for notification purposes
        public ConcurrentDictionary<string, ConcurrentQueue<PlayerMessage>> dms { get; } = new ConcurrentDictionary<string, ConcurrentQueue<PlayerMessage>>();//brian - i added this for dms, using the string of the target player, we can get the queue of the dms
    }
}
