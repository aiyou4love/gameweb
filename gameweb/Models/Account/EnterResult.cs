﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gameweb
{
    public class EnterResult
    {
        public int mSessionId { get; set; }
        public int mErrorCode { get; set; }
        public long mAccountId { get; set; }
        public short mAuthority { get; set; }
        public RoleItem mRoleItem { get; set; }
    }
}
