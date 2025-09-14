using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MortalCombatXLibrary
{
    public class ChatMessage
    {
        public string DisplayText { get; set; }
        public bool IsFile { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; } //.jpg.txt etc
        public string FileData { get; set; }

    }
}
