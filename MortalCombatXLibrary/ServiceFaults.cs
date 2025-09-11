using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace MortalCombatXLibrary
{
    [DataContract]
    public class UsernameExistsFault
    {
        [DataMember]
        public string Message { get; set; }
    }

    [DataContract]
    public class LobbyExistsFault
    {
        [DataMember]
        public string Message { get; set; }
    }
}
