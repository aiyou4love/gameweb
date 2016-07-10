using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gameweb
{
    public class LoginResult
    {
        public long mAccountId { get; set; }
        public RoleItem mRoleItem { get; set; }
        public ServerItem mServerItem { get; set; }
    }
}
