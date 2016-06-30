using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace gameweb
{
    public class AgentAspect
    {
        public static AgentInfo getIdleAgent(string nOperatorName, int nVersionNo)
        {
            initAgent();

            string operatorName_ = OperatorAspect.getOperator(nOperatorName, nVersionNo);
            if ("" == operatorName_) return null;

            return mAgentStates[operatorName_].getIdleAgent();
        }

        static string mInitAgent = "SELECT operatorName,agentId,agentIp,agentPort,playerMax,playerCount FROM t_agentList";
        public static void initAgent()
        {
            if (mInitAgented) return;

            SqlConnection sqlConnection_ = new SqlConnection();

            sqlConnection_.ConnectionString = ConstAspect.mConnectionString;
            sqlConnection_.Open();

            SqlCommand sqlCommand_ = new SqlCommand();
            sqlCommand_.Connection = sqlConnection_;
            sqlCommand_.CommandType = CommandType.Text;
            sqlCommand_.CommandText = mInitAgent;
            SqlDataReader sqlDataReader_ = sqlCommand_.ExecuteReader();
            while (sqlDataReader_.Read())
            {
                AgentInfo agentInfo_ = new AgentInfo();
                string operatorName_ = sqlDataReader_.GetString(0).Trim();
                int agentId_ = sqlDataReader_.GetInt32(1);
                agentInfo_.mAgentIp = sqlDataReader_.GetString(2).Trim();
                agentInfo_.mAgentPort = sqlDataReader_.GetInt32(3);
                agentInfo_.mPlayerMax = sqlDataReader_.GetInt32(4);
                agentInfo_.mPlayerCount = sqlDataReader_.GetInt32(5);
                if (!mAgentStates.ContainsKey(operatorName_))
                {
                    AgentState agentState_ = new AgentState();
                    mAgentStates[operatorName_] = agentState_;
                }
                mAgentStates[operatorName_].pushAgentInfo(agentId_, agentInfo_);
            }
            sqlDataReader_.Close();
            sqlConnection_.Close();
            mInitAgented = true;
        }

        static Dictionary<string, AgentState> mAgentStates = new Dictionary<string, AgentState>();

        static bool mInitAgented = false;
    }
}
