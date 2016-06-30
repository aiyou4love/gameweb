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
        static string mServerId = "SELECT MAX(serverId) FROM t_serverList WHERE operatorName='{0}'";
        public static int getServerId(string nOperatorName, int nVersionNo)
        {
            string operatorName_ = OperatorAspect.getOperator(nOperatorName, nVersionNo);
            if ("" == operatorName_) return 0;

            SqlConnection sqlConnection_ = new SqlConnection();

            sqlConnection_.ConnectionString = ConstAspect.mConnectionString;
            sqlConnection_.Open();

            SqlCommand sqlCommand_ = new SqlCommand();
            sqlCommand_.Connection = sqlConnection_;
            sqlCommand_.CommandType = CommandType.Text;
            sqlCommand_.CommandText = string.Format(mServerId, operatorName_);
            SqlDataReader sqlDataReader_ = sqlCommand_.ExecuteReader();
            int serverId_ = 0;
            if (sqlDataReader_.Read())
            {
                serverId_ = sqlDataReader_.GetInt32(0);
            }
            sqlDataReader_.Close();
            sqlConnection_.Close();
            return serverId_;
        }

        static string mServerName = "SELECT serverName,serverNo FROM t_serverList WHERE operatorName='{0}' AND serverId='{1}';";
        public static ServerItem getServerItem(string nOperatorName, int nVersionNo, int nServerId)
        {
            string operatorName_ = OperatorAspect.getOperator(nOperatorName, nVersionNo);
            if ("" == operatorName_) return null;

            SqlConnection sqlConnection_ = new SqlConnection();

            sqlConnection_.ConnectionString = ConstAspect.mConnectionString;
            sqlConnection_.Open();

            SqlCommand sqlCommand_ = new SqlCommand();
            sqlCommand_.Connection = sqlConnection_;
            sqlCommand_.CommandType = CommandType.Text;
            sqlCommand_.CommandText = string.Format(mServerName, operatorName_, nServerId);
            SqlDataReader sqlDataReader_ = sqlCommand_.ExecuteReader();
            ServerItem serverItem_ = null;
            if (sqlDataReader_.Read())
            {
                serverItem_ = new ServerItem();
                serverItem_.mServerId = nServerId;
                serverItem_.mServerName = sqlDataReader_.GetString(0).Trim();
                serverItem_.mServerNo = sqlDataReader_.GetInt32(2);
            }
            sqlDataReader_.Close();
            sqlConnection_.Close();
            return serverItem_;
        }

        static string mServerList = "SELECT serverId,serverName,serverNo FROM t_serverList WHERE operatorName='{0}';";
        public static List<ServerItem> getServerList(string nOperatorName, int nVersionNo)
        {
            string operatorName_ = OperatorAspect.getOperator(nOperatorName, nVersionNo);
            if ("" == operatorName_) return null;

            SqlConnection sqlConnection_ = new SqlConnection();

            sqlConnection_.ConnectionString = ConstAspect.mConnectionString;
            sqlConnection_.Open();

            List<ServerItem> serverItems_ = null;

            SqlCommand sqlCommand_ = new SqlCommand();
            sqlCommand_.Connection = sqlConnection_;
            sqlCommand_.CommandType = CommandType.Text;
            sqlCommand_.CommandText = string.Format(mServerList, operatorName_);
            SqlDataReader sqlDataReader_ = sqlCommand_.ExecuteReader();
            while (sqlDataReader_.Read())
            {
                if (null == serverItems_)
                {
                    serverItems_ = new List<ServerItem>();
                }
                ServerItem serverItem_ = new ServerItem();
                serverItem_.mServerId = sqlDataReader_.GetInt32(0);
                serverItem_.mServerName = sqlDataReader_.GetString(1).Trim();
                serverItem_.mServerNo = sqlDataReader_.GetInt32(2);
                serverItems_.Add(serverItem_);
            }
            sqlDataReader_.Close();
            sqlConnection_.Close();
            return serverItems_;
        }

        public static ServerInfo getServerInfo(string nOperatorName, int nVersionNo, int nServerId)
        {
            initServerState();

            string operatorName_ = OperatorAspect.getOperator(nOperatorName, nVersionNo);
            if ("" == operatorName_) return null;

            return mServerStates[operatorName_].getServerInfo(nServerId);
        }

        static string mInitServerNos = "SELECT operatorName,serverId,serverNo FROM t_serverList";
        static void initServerNos()
        {
            SqlConnection sqlConnection_ = new SqlConnection();

            sqlConnection_.ConnectionString = ConstAspect.mConnectionString;
            sqlConnection_.Open();

            SqlCommand sqlCommand_ = new SqlCommand();
            sqlCommand_.Connection = sqlConnection_;
            sqlCommand_.CommandType = CommandType.Text;
            sqlCommand_.CommandText = mInitServerNos;
            SqlDataReader sqlDataReader_ = sqlCommand_.ExecuteReader();
            while (sqlDataReader_.Read())
            {
                string operatorName_ = sqlDataReader_.GetString(0).Trim();
                int serverId_ = sqlDataReader_.GetInt32(1);
                int serverNo_ = sqlDataReader_.GetInt32(2);
                if (!mServerStates.ContainsKey(operatorName_))
                {
                    ServerState serverState_ = new ServerState();
                    mServerStates[operatorName_] = serverState_;
                }
                mServerStates[operatorName_].pushServerNo(serverId_, serverNo_);
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
                string serverStart_ = sqlDataReader_.GetString(1).Trim();
                serverInfo_.mServerStart = Convert.ToDateTime(serverStart_);
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

        static void initServerState()
        {
            if (mInitServered) return;

            initServerNos();
            initServerInfo();

            mInitServered = true;
        }

        static Dictionary<string, ServerState> mServerStates = new Dictionary<string, ServerState>();

        static bool mInitServered = false;
    }
}
