using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace gameweb
{
    public class AccountAspect
    {
        static string mAccountRegister = @"INSERT INTO t_accountTb(accountName,accountPassword,accountType)VALUES('{0}','{1}','{2}');";
        public static bool accountRegister(string nAccountName, string nAccountPassword, int nAccountType)
        {
            SqlConnection sqlConnection_ = new SqlConnection();

            sqlConnection_.ConnectionString = ConstAspect.mConnectionString;
            sqlConnection_.Open();

            SqlCommand sqlCommand_ = new SqlCommand();
            sqlCommand_.Connection = sqlConnection_;
            sqlCommand_.CommandType = CommandType.Text;
            sqlCommand_.CommandText = string.Format(mAccountRegister, nAccountName, nAccountPassword, nAccountType);
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
        static string mAccountCheck = "SELECT COUNT(*) FROM t_accountTb WHERE accountName='{0}' AND accountType='1';";
        public static bool accountCheck(string nAccountName)
        {
            SqlConnection sqlConnection_ = new SqlConnection();

            sqlConnection_.ConnectionString = ConstAspect.mConnectionString;
            sqlConnection_.Open();

            SqlCommand sqlCommand_ = new SqlCommand();
            sqlCommand_.Connection = sqlConnection_;
            sqlCommand_.CommandType = CommandType.Text;
            sqlCommand_.CommandText = string.Format(mAccountCheck, nAccountName);
            SqlDataReader sqlDataReader_ = sqlCommand_.ExecuteReader();
            int value_ = 0;
            if (sqlDataReader_.Read())
            {
                value_ = sqlDataReader_.GetInt32(0);
            }
            sqlDataReader_.Close();
            sqlConnection_.Close();
            return (1 == value_);
        }
        public static long getAccountId(string nAccountName, string nPassword, short nAccountType)
        {
            if (1 == nAccountType)
            {
                return getGameAccount(nAccountName, nPassword, nAccountType);
            }
            if (!validAccount(nAccountName, nPassword, nAccountType))
            {
                return 0;
            }
            long accountId_ = getGameAccount(nAccountName, nPassword, nAccountType);

            if (accountId_ > 0) return accountId_;

            if (accountRegister(nAccountName, "3SVkxs8b0Bj4kgqo", nAccountType))
            {
                return getGameAccount(nAccountName, nPassword, nAccountType);
            }
            return 0;
        }
        static string mGameAccount = "SELECT accountId,accountPassword FROM t_accountTb WHERE accountName='{0}' AND accountType='{1}';";
        public static long getGameAccount(string nAccountName, string nPassword, short nAccountType)
        {
            SqlConnection sqlConnection_ = new SqlConnection();

            sqlConnection_.ConnectionString = ConstAspect.mConnectionString;
            sqlConnection_.Open();

            SqlCommand sqlCommand_ = new SqlCommand();
            sqlCommand_.Connection = sqlConnection_;
            sqlCommand_.CommandType = CommandType.Text;
            sqlCommand_.CommandText = string.Format(mGameAccount, nAccountName, nAccountType);
            SqlDataReader sqlDataReader_ = sqlCommand_.ExecuteReader();
            long accountId_ = 0;
            string password_ = "";
            if (sqlDataReader_.Read())
            {
                accountId_ = sqlDataReader_.GetInt64(0);
                password_ = sqlDataReader_.GetString(1).Trim();
            }
            sqlDataReader_.Close();
            sqlConnection_.Close();

            if (1 == nAccountType)
            {
                if (("" == password_) || (nPassword != password_))
                {
                    accountId_ = 0;
                }
            }
            return accountId_;
        }
        public static bool validAccount(string nAccountName, string nPassword, short nAccountType)
        {
            return true;
        }
    }
}
