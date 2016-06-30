using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gameweb
{
    public class ServerState
    {
        public void pushServerInfo(int nServerNo, ServerInfo nServerInfo)
        {
            mServerInfos[nServerNo] = nServerInfo;
        }

        public void pushServerNo(int nServerId, int nServerNo)
        {
            mServerNos[nServerId] = nServerNo;
        }

        public ServerInfo getServerInfo(int nServerId)
        {
            if (!mServerNos.ContainsKey(nServerId))
            {
                return null;
            }
            int serverNo_ = mServerNos[nServerId];
            if (!mServerInfos.ContainsKey(serverNo_))
            {
                return null;
            }
            return mServerInfos[serverNo_];
        }

        static Dictionary<int, ServerInfo> mServerInfos = new Dictionary<int, ServerInfo>();
        static Dictionary<int, int> mServerNos = new Dictionary<int, int>();
    }
}
