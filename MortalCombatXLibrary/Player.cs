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
        //idk if we need anything else for the player class
        public Player(string username)
        {
            Username = username;
        }
    }
}
