using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gameweb
{
    public class LoginResult
    {
        public long mAccountId { get; set; }
        public PlayerItem mPlayerItem { get; set; }
        public ServerItem mServerItem { get; set; }
    }
}
