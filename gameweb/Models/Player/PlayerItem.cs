using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gameweb
{
    public class PlayerItem
    {
        public int mServerId { get; set; }
        public int mPlayerId { get; set; }
        public short mPlayerType { get; set; }
        public string mPlayerName { get; set; }
        public short mPlayerRace { get; set; }
        public int mPlayerLevel { get; set; }
    }
}
