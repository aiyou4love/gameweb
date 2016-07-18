using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gameweb
{
    public class RoleRequest
    {
        public string mAccountName { get; set; }
        public string mPassword { get; set; }
        public short mAccountType { get; set; }
        public string mOperatorName { get; set; }
        public int mVersionNo { get; set; }
        public long mAccountId { get; set; }
        public int mServerId { get; set; }
        public string mRoleName { get; set; }
        public short mRoleRace { get; set; }
        public bool mUpdate { get; set; }
    }
}
