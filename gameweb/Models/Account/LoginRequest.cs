using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gameweb
{
    public class LoginRequest
    {
        public string mAccountName { get; set; }
        public string mPassword { get; set; }
        public string mOperatorName { get; set; }
        public int mVersionNo { get; set; }
        public short mAccountType { get; set; }
    }
}
