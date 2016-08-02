using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace gameweb.Controllers
{
    public class AutoupController : ApiController
    {
        //http://localhost:8313/api/autoup/lastupdate/?nOperatorName=iosfigus&nVersionNo=1
        readonly string mLastUpdate = "SELECT updateName,updateNo,packetName,downloadUrl FROM t_lastUpdate WHERE operatorName='{0}'";
        [HttpGet]
        public HttpResponseMessage lastUpdate(string nOperatorName, int nVersionNo)
        {
            UpdateResult updateResult_ = new UpdateResult();
            updateResult_.mErrorCode = ConstAspect.mFail;
            updateResult_.mUpdateItems = null;

            SqlConnection sqlConnection_ = new SqlConnection();

            sqlConnection_.ConnectionString = ConstAspect.mConnectionString;
            sqlConnection_.Open();

            SqlCommand sqlCommand_ = new SqlCommand();
            sqlCommand_.Connection = sqlConnection_;
            sqlCommand_.CommandType = CommandType.Text;
            sqlCommand_.CommandText = string.Format(mLastUpdate, nOperatorName);
            SqlDataReader sqlDataReader_ = sqlCommand_.ExecuteReader();
            while (sqlDataReader_.Read())
            {
                updateResult_.mErrorCode = ConstAspect.mSucess;

                string updateName_ = sqlDataReader_.GetString(0).Trim();
                if ("versionNo" != updateName_)
                {
                    if (null == updateResult_.mUpdateItems)
                    {
                        updateResult_.mUpdateItems = new List<UpdateItem>();
                    }
                    UpdateItem updateItem_ = new UpdateItem();
                    updateItem_.mUpdateName = updateName_;
                    updateItem_.mUpdateNo = sqlDataReader_.GetInt32(1);
                    updateItem_.mPacketName = sqlDataReader_.GetString(2).Trim();
                    updateItem_.mDownloadUrl = sqlDataReader_.GetString(3).Trim();
                    updateResult_.mUpdateItems.Add(updateItem_);
                }
                else
                {
                    updateResult_.mVersionNo = sqlDataReader_.GetInt32(1);
                }
            }
            sqlDataReader_.Close();
            sqlConnection_.Close();
            return toJson(updateResult_);
        }

        HttpResponseMessage toJson(Object nObject)
        {
            String value_;
            if (nObject is String || nObject is Char)
            {
                value_ = nObject.ToString();
            }
            else
            {
                value_ = JsonConvert.SerializeObject(nObject);
            }
            HttpResponseMessage result_ = new HttpResponseMessage();
            result_.Content = new StringContent(value_, Encoding.GetEncoding("UTF-8"), "application/json");
            return result_;
        }
    }
}
