using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gameweb
{
    public class EnterRequest
    {
        public int mSessionId { get; set; }
        public string mAccountName { get; set; }
        public string mPassword { get; set; }
        public short mAccountType { get; set; }
        public string mOperatorName { get; set; }
        public int mVersionNo { get; set; }
        public long mAccountId { get; set; }
        public int mPlayerId { get; set; }
        public int mServerId { get; set; }
        public bool mStart { get; set; }
    }
}
