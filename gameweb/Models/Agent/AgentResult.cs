using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gameweb
{
    public class AgentResult
    {
        public int mErrorCode { get; set; }
        public string mAgentIp { get; set; }
        public string mAgentPort { get; set; }
        public ServerInfo mServerInfo { get; set; }
    }
}
