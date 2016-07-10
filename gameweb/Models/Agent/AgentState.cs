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
                if (i.Value.mRoleCount < i.Value.mRoleMax)
                {
                    return i.Value;
                }
            }
            return null;
        }

        public int getRoleCount(int nAgentId)
        {
            return mAgentInfos[nAgentId].mRoleCount;
        }

        public void roleEnter(int nAgentId)
        {
            mAgentInfos[nAgentId].mRoleCount++;
        }

        Dictionary<int, AgentInfo> mAgentInfos = new Dictionary<int, AgentInfo>();
    }
}
