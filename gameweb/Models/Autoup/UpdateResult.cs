using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gameweb
{
    public class UpdateResult
    {
        public int mErrorCode { get; set; }
        public int mVersionNo { get; set; }
        public List<UpdateItem> mUpdateItems { get; set; }
    }
}
