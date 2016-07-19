using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace gameweb
{
    public class ServerAspect
    {
        public static int getServerId(string nOperatorName, int nVersionNo)
        {
            initServerState(false);

            string operatorName_ = OperatorAspect.getOperator(nOperatorName, nVersionNo);
            if ("" == operatorName_) return 0;

            return mServerStates[operatorName_].getServerId();
        }

        public static ServerItem getServerItem(string nOperatorName, int nVersionNo, int nServerId)
        {
            initServerState(false);

            string operatorName_ = OperatorAspect.getOperator(nOperatorName, nVersionNo);
            if ("" == operatorName_) return null;

            return mServerStates[operatorName_].getServerItem(nServerId);
        }

        public static List<ServerItem> getServerList(string nOperatorName, int nVersionNo)
        {
            initServerState(false);

            string operatorName_ = OperatorAspect.getOperator(nOperatorName, nVersionNo);
            if ("" == operatorName_) return null;

            return mServerStates[operatorName_].getServerList();
        }

        public static ServerInfo getServerInfo(string nOperatorName, int nVersionNo, int nServerId)
        {
            initServerState(false);

            string operatorName_ = OperatorAspect.getOperator(nOperatorName, nVersionNo);
            if ("" == operatorName_) return null;

            return mServerStates[operatorName_].getServerInfo(nServerId);
        }

        static string mInitServerItems = "SELECT operatorName,serverId,serverNo,serverName FROM t_serverList";
        static void initServerItems()
        {
            SqlConnection sqlConnection_ = new SqlConnection();

            sqlConnection_.ConnectionString = ConstAspect.mConnectionString;
            sqlConnection_.Open();

            SqlCommand sqlCommand_ = new SqlCommand();
            sqlCommand_.Connection = sqlConnection_;
            sqlCommand_.CommandType = CommandType.Text;
            sqlCommand_.CommandText = mInitServerItems;
            SqlDataReader sqlDataReader_ = sqlCommand_.ExecuteReader();
            while (sqlDataReader_.Read())
            {
                string operatorName_ = sqlDataReader_.GetString(0).Trim();
                ServerItem serverItem_ = new ServerItem();
                serverItem_.mServerId = sqlDataReader_.GetInt32(1);
                serverItem_.mServerNo = sqlDataReader_.GetInt32(2);
                serverItem_.mServerName = sqlDataReader_.GetString(3).Trim();
                if (!mServerStates.ContainsKey(operatorName_))
                {
                    ServerState serverState_ = new ServerState();
                    mServerStates[operatorName_] = serverState_;
                }
                mServerStates[operatorName_].pushServerItem(serverItem_);
            }
            sqlDataReader_.Close();
            sqlConnection_.Close();
        }

        static string mServerInfo = "SELECT operatorName,serverNo,serverStart FROM t_serverInfo";
        static void initServerInfo()
        {
            SqlConnection sqlConnection_ = new SqlConnection();

            sqlConnection_.ConnectionString = ConstAspect.mConnectionString;
            sqlConnection_.Open();

            SqlCommand sqlCommand_ = new SqlCommand();
            sqlCommand_.Connection = sqlConnection_;
            sqlCommand_.CommandType = CommandType.Text;
            sqlCommand_.CommandText = mServerInfo;
            SqlDataReader sqlDataReader_ = sqlCommand_.ExecuteReader();
            while (sqlDataReader_.Read())
            {
                ServerInfo serverInfo_ = new ServerInfo();
                string operatorName_ = sqlDataReader_.GetString(0).Trim();
                int serverNo_ = sqlDataReader_.GetInt32(1);
                string serverStart_ = sqlDataReader_.GetString(2).Trim();
                DateTime dateTime_ = Convert.ToDateTime(serverStart_);
                serverInfo_.mServerStart = ToTimestamp(dateTime_);
                if (!mServerStates.ContainsKey(operatorName_))
                {
                    ServerState serverState_ = new ServerState();
                    mServerStates[operatorName_] = serverState_;
                }
                mServerStates[operatorName_].pushServerInfo(serverNo_, serverInfo_);
            }
            sqlDataReader_.Close();
            sqlConnection_.Close();
        }

        static long ToTimestamp(DateTime value)
        {
            TimeSpan span = (value - new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime());
            return (long)span.TotalSeconds;
        }

        static DateTime ConvertTimestamp(long timestamp)
        {
            DateTime converted = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            DateTime newDateTime = converted.AddSeconds(timestamp);
            return newDateTime.ToLocalTime();
        }

        static void initServerState(bool nReinit)
        {
            if (!nReinit)
            {
                if (mInitServered) return;
            }
            else
            {
                mServerStates.Clear();
            }

            initServerItems();
            initServerInfo();

            mInitServered = true;
        }

        static Dictionary<string, ServerState> mServerStates = new Dictionary<string, ServerState>();

        static bool mInitServered = false;
    }
}
