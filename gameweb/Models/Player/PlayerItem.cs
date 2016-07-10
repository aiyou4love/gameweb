using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gameweb
{
    public class RoleItem
    {
        public int mServerId { get; set; }
        public int mRoleId { get; set; }
        public short mRoleType { get; set; }
        public string mRoleName { get; set; }
        public short mRoleRace { get; set; }
        public short mRoleStep { get; set; }
        public int mRoleLevel { get; set; }
    }
}
