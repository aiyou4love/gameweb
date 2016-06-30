using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gameweb
{
    public class AgentState
    {
        public void pushAgentInfo(int nAgentId, AgentInfo nAgentInfo)
        {
            mAgentInfos[nAgentId] = nAgentInfo;
        }

        public AgentInfo getIdleAgent()
        {
            foreach (var i in mAgentInfos)
            {
                if (i.Value.mPlayerCount < i.Value.mPlayerMax)
                {
                    return i.Value;
                }
            }
            return null;
        }

        public int getPlayerCount(int nAgentId)
        {
            return mAgentInfos[nAgentId].mPlayerCount;
        }

        public void playerEnter(int nAgentId)
        {
            mAgentInfos[nAgentId].mPlayerCount++;
        }

        Dictionary<int, AgentInfo> mAgentInfos = new Dictionary<int, AgentInfo>();
    }
}
