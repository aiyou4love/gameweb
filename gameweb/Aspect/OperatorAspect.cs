using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace gameweb
{
    public class OperatorAspect
    {
        public static string getOperator(string nOperatorName, int nVersionNo)
        {
            initOperator();

            foreach (OperatorInfo i in mOperatorInfos)
            {
                if ( (i.mOperatorKey == nOperatorName) && (i.mVersionNo == nVersionNo) )
                {
                    return i.mOperatorName;
                }
            }
            return "";
        }

        static string mInitOperator = @"SELECT operatorKey,versionNo,operatorName FROM t_operatorName;";
        static void initOperator()
        {
            if (mInitOperatored) return;

            SqlConnection sqlConnection_ = new SqlConnection();

            sqlConnection_.ConnectionString = ConstAspect.mConnectionString;
            sqlConnection_.Open();

            SqlCommand sqlCommand_ = new SqlCommand();
            sqlCommand_.Connection = sqlConnection_;
            sqlCommand_.CommandType = CommandType.Text;
            sqlCommand_.CommandText = mInitOperator;
            SqlDataReader sqlDataReader_ = sqlCommand_.ExecuteReader();
            while (sqlDataReader_.Read())
            {
                OperatorInfo operatorInfo_ = new OperatorInfo();
                operatorInfo_.mOperatorKey = sqlDataReader_.GetString(0).Trim();
                operatorInfo_.mVersionNo = sqlDataReader_.GetInt32(1);
                operatorInfo_.mOperatorName = sqlDataReader_.GetString(2).Trim();
                mOperatorInfos.Add(operatorInfo_);
            }
            sqlDataReader_.Close();
            sqlConnection_.Close();
            mInitOperatored = true;
        }

        static List<OperatorInfo> mOperatorInfos = new List<OperatorInfo>();

        static bool mInitOperatored = false;
    }
}
