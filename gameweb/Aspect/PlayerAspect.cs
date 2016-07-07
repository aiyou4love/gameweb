using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace gameweb
{
    public class PlayerAspect
    {
        static string mCreatePlayer = "INSERT INTO t_playerList(operatorName,accountId,serverId,playerId,playerName,playerRace,playerType,playerStep,playerLevel)VALUES('{0}','{1}','{2}','{3}','{4}','{5}','1','1','1');";
        public static bool createPlayer(string nOperatorName, int nVersionNo, long nAccountId, int nServerId, string nPlayerName, short nPlayerRace)
        {
            string operatorName_ = OperatorAspect.getOperator(nOperatorName, nVersionNo);
            if ("" == operatorName_) return false;

            SqlConnection sqlConnection_ = new SqlConnection();

            sqlConnection_.ConnectionString = ConstAspect.mConnectionString;
            sqlConnection_.Open();

            SqlCommand sqlCommand_ = new SqlCommand();
            sqlCommand_.Connection = sqlConnection_;
            sqlCommand_.CommandType = CommandType.Text;
            sqlCommand_.CommandText = string.Format(mCreatePlayer, operatorName_, nAccountId, nServerId, nServerId, nPlayerName, nPlayerRace);
            bool result_ = false;
            try
            {
                sqlCommand_.ExecuteNonQuery();
                result_ = true;
            }
            catch (SqlException)
            {
                result_ = false;
            }
            sqlConnection_.Close();
            return result_;
        }

        static string mPlayerCount = "SELECT COUNT(*) FROM t_playerList WHERE operatorName='{0}' AND accountId='{1}' AND serverId='{2}';";
        public static int getPlayerCount(string nOperatorName, int nVersionNo, long nAccountId, int nServerId)
        {
            string operatorName_ = OperatorAspect.getOperator(nOperatorName, nVersionNo);
            if ("" == operatorName_) return 0;

            SqlConnection sqlConnection_ = new SqlConnection();

            sqlConnection_.ConnectionString = ConstAspect.mConnectionString;
            sqlConnection_.Open();

            SqlCommand sqlCommand_ = new SqlCommand();
            sqlCommand_.Connection = sqlConnection_;
            sqlCommand_.CommandType = CommandType.Text;
            sqlCommand_.CommandText = string.Format(mPlayerCount, operatorName_, nAccountId, nServerId);
            SqlDataReader sqlDataReader_ = sqlCommand_.ExecuteReader();
            int playerCount_ = 0;
            if (sqlDataReader_.Read())
            {
                playerCount_ = sqlDataReader_.GetInt32(0);
            }
            sqlDataReader_.Close();
            sqlConnection_.Close();
            return playerCount_;
        }

        static string mUpdatePlayerStart = "UPDATE t_playerStart SET serverId='{0}',playerId='{1}' WHERE operatorName='{2}' AND accountId='{3}';";
        public static int updatePlayerStart(string nOperatorName, int nVersionNo, long nAccountId, int nServerId, int nPlayerId)
        {
            string operatorName_ = OperatorAspect.getOperator(nOperatorName, nVersionNo);
            if ("" == operatorName_) return ConstAspect.mOperator;

            int result_ = ConstAspect.mFail;

            SqlConnection sqlConnection_ = new SqlConnection();

            sqlConnection_.ConnectionString = ConstAspect.mConnectionString;
            sqlConnection_.Open();

            SqlCommand sqlCommand_ = new SqlCommand();
            sqlCommand_.Connection = sqlConnection_;
            sqlCommand_.CommandType = CommandType.Text;
            sqlCommand_.CommandText = string.Format(mUpdatePlayerStart, nServerId, nPlayerId, operatorName_, nAccountId);
            try
            {
                sqlCommand_.ExecuteNonQuery();
                result_ = ConstAspect.mSucess;
            }
            catch (SqlException)
            {
                result_ = ConstAspect.mSql;
            }
            sqlConnection_.Close();
            return result_;
        }

        static string mInsertPlayerStart = "INSERT INTO t_playerStart(operatorName,accountId,serverId,playerId) VALUES ('{0}','{1}','{2}','{3}');";
        public static int insertPlayerStart(string nOperatorName, int nVersionNo, long nAccountId, int nServerId, int nPlayerId)
        {
            string operatorName_ = OperatorAspect.getOperator(nOperatorName, nVersionNo);
            if ("" == operatorName_) return ConstAspect.mOperator;

            int result_ = ConstAspect.mFail;

            SqlConnection sqlConnection_ = new SqlConnection();

            sqlConnection_.ConnectionString = ConstAspect.mConnectionString;
            sqlConnection_.Open();

            SqlCommand sqlCommand_ = new SqlCommand();
            sqlCommand_.Connection = sqlConnection_;
            sqlCommand_.CommandType = CommandType.Text;
            sqlCommand_.CommandText = string.Format(mInsertPlayerStart, operatorName_, nAccountId, nServerId, nPlayerId);
            try
            {
                sqlCommand_.ExecuteNonQuery();
                result_ = ConstAspect.mSucess;
            }
            catch (SqlException)
            {
                result_ = ConstAspect.mSql;
            }
            sqlConnection_.Close();
            return result_;
        }

        static string mPlayerInfo = "SELECT playerType,playerName,playerRace,playerStep,playerLevel FROM t_playerList WHERE operatorName='{0}' AND accountId='{1}' AND playerId='{2}' AND serverId='{3}';";
        public static PlayerItem getPlayerInfo(string nOperatorName, int nVersionNo, long nAccountId, int nPlayerId, int nServerId)
        {
            string operatorName_ = OperatorAspect.getOperator(nOperatorName, nVersionNo);
            if ("" == operatorName_) return null;

            PlayerItem playerItem_ = null;

            SqlConnection sqlConnection_ = new SqlConnection();

            sqlConnection_.ConnectionString = ConstAspect.mConnectionString;
            sqlConnection_.Open();

            SqlCommand sqlCommand_ = new SqlCommand();
            sqlCommand_.Connection = sqlConnection_;
            sqlCommand_.CommandType = CommandType.Text;
            sqlCommand_.CommandText = string.Format(mPlayerInfo, operatorName_, nAccountId, nPlayerId, nServerId);
            SqlDataReader sqlDataReader_ = sqlCommand_.ExecuteReader();
            if (sqlDataReader_.Read())
            {
                playerItem_ = new PlayerItem();
                playerItem_.mServerId = nServerId;
                playerItem_.mPlayerId = nPlayerId;
                playerItem_.mPlayerType = sqlDataReader_.GetInt16(0);
                playerItem_.mPlayerName = sqlDataReader_.GetString(1).Trim();
                playerItem_.mPlayerRace = sqlDataReader_.GetInt16(2);
                playerItem_.mPlayerStep = sqlDataReader_.GetInt16(3);
                playerItem_.mPlayerLevel = sqlDataReader_.GetInt32(4);
            }
            sqlDataReader_.Close();
            sqlConnection_.Close();
            return playerItem_;
        }

        static string mPlayerStart = "SELECT serverId,playerId FROM t_playerStart WHERE operatorName='{0}' AND accountId='{1}';";
        public static PlayerStart getPlayerStart(string nOperatorName, int nVersionNo, long nAccountId)
        {
            string operatorName_ = OperatorAspect.getOperator(nOperatorName, nVersionNo);
            if ("" == operatorName_) return null;

            SqlConnection sqlConnection_ = new SqlConnection();

            sqlConnection_.ConnectionString = ConstAspect.mConnectionString;
            sqlConnection_.Open();

            SqlCommand sqlCommand_ = new SqlCommand();
            sqlCommand_.Connection = sqlConnection_;
            sqlCommand_.CommandType = CommandType.Text;
            sqlCommand_.CommandText = string.Format(mPlayerStart, operatorName_, nAccountId);
            SqlDataReader sqlDataReader_ = sqlCommand_.ExecuteReader();
            PlayerStart playerStartInfo_ = null;
            if (sqlDataReader_.Read())
            {
                playerStartInfo_ = new PlayerStart();
                playerStartInfo_.mServerId = sqlDataReader_.GetInt32(0);
                playerStartInfo_.mPlayerId = sqlDataReader_.GetInt32(1);
            }
            sqlDataReader_.Close();
            sqlConnection_.Close();
            return playerStartInfo_;
        }

        static string mPlayerList = "SELECT serverId,playerId,playerType,playerName,playerRace,playerStep,playerLevel FROM t_playerList WHERE operatorName='{0}' AND accountId='{1}';";
        public static List<PlayerItem> getPlayerList(string nOperatorName, int nVersionNo, long nAccountId)
        {
            string operatorName_ = OperatorAspect.getOperator(nOperatorName, nVersionNo);
            if ("" == operatorName_) return null;

            List<PlayerItem> playerItems_ = null;

            SqlConnection sqlConnection_ = new SqlConnection();

            sqlConnection_.ConnectionString = ConstAspect.mConnectionString;
            sqlConnection_.Open();

            SqlCommand sqlCommand_ = new SqlCommand();
            sqlCommand_.Connection = sqlConnection_;
            sqlCommand_.CommandType = CommandType.Text;
            sqlCommand_.CommandText = string.Format(mPlayerList, operatorName_, nAccountId);
            SqlDataReader sqlDataReader_ = sqlCommand_.ExecuteReader();
            while (sqlDataReader_.Read())
            {
                if (null == playerItems_)
                {
                    playerItems_ = new List<PlayerItem>();
                }
                PlayerItem playerItem_ = new PlayerItem();
                playerItem_.mServerId = sqlDataReader_.GetInt32(0);
                playerItem_.mPlayerId = sqlDataReader_.GetInt32(1);
                playerItem_.mPlayerType = sqlDataReader_.GetInt16(2);
                playerItem_.mPlayerName = sqlDataReader_.GetString(3).Trim();
                playerItem_.mPlayerRace = sqlDataReader_.GetInt16(4);
                playerItem_.mPlayerStep = sqlDataReader_.GetInt16(5);
                playerItem_.mPlayerLevel = sqlDataReader_.GetInt32(6);
                playerItems_.Add(playerItem_);
            }
            sqlDataReader_.Close();
            sqlConnection_.Close();
            return playerItems_;
        }
    }
}
