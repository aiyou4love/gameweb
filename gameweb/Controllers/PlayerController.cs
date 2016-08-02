using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using Newtonsoft.Json;

namespace gameweb.Controllers
{
    public class RoleController : ApiController
    {
        //http://localhost:8313/api/values/createRole
        //content-type: application/json;charset=utf-8
        //{"mAccountName": "zyh", "mAccountPassword": "123456", "mAgentName": "iosfigus", "mVersionNo": "1","mAccountId": "1", "mServerId": "1", "mRoleName": "赵由华", "nRoleRace": "1", "nUpdate": "true"}
        [HttpPost]
        public HttpResponseMessage createRole([FromBody]RoleRequest nRoleRequest)
        {
            AccountInfo accountInfo_ = AccountAspect.getAccountId(nRoleRequest.mAccountName, nRoleRequest.mPassword, nRoleRequest.mAccountType);
            RoleResult roleResult_ = new RoleResult();
            roleResult_.mErrorCode = ConstAspect.mFail;
            roleResult_.mAccountId = 0;
            roleResult_.mRoleItem = null;
            roleResult_.mServerItem = null;
            if ((null == accountInfo_) || (0 == accountInfo_.mAccountId) || (nRoleRequest.mAccountId != accountInfo_.mAccountId))
            {
                roleResult_.mErrorCode = ConstAspect.mAccount;
                return toJson(roleResult_);
            }
            roleResult_.mAccountId = accountInfo_.mAccountId;
            int roleCount_ = RoleAspect.getRoleCount(nRoleRequest.mOperatorName, nRoleRequest.mVersionNo, accountInfo_.mAccountId, nRoleRequest.mServerId);
            if (roleCount_ > 0)
            {
                roleResult_.mErrorCode = ConstAspect.mRole;
                return toJson(roleResult_);
            }
            if (RoleAspect.createRole(nRoleRequest.mOperatorName, nRoleRequest.mVersionNo, accountInfo_.mAccountId, nRoleRequest.mServerId, nRoleRequest.mRoleName, nRoleRequest.mRoleRace))
            {
                roleResult_.mErrorCode = ConstAspect.mSucess;
                roleResult_.mAccountId = accountInfo_.mAccountId;
                roleResult_.mRoleItem = new RoleItem();
                roleResult_.mRoleItem.mRoleId = nRoleRequest.mServerId;
                roleResult_.mRoleItem.mServerId = nRoleRequest.mServerId;
                roleResult_.mRoleItem.mRoleName = nRoleRequest.mRoleName;
                roleResult_.mRoleItem.mRoleRace = nRoleRequest.mRoleRace;
                roleResult_.mRoleItem.mRoleStep = 1;
                roleResult_.mRoleItem.mRoleLevel = 1;
                roleResult_.mRoleItem.mRoleType = 1;
                roleResult_.mServerItem = ServerAspect.getServerItem(nRoleRequest.mOperatorName, nRoleRequest.mVersionNo, nRoleRequest.mServerId);
            }
            else
            {
                roleResult_.mErrorCode = ConstAspect.mCreate;
            }
            if (nRoleRequest.mUpdate)
            {
                RoleAspect.updateRoleStart(nRoleRequest.mOperatorName, nRoleRequest.mVersionNo, accountInfo_.mAccountId, nRoleRequest.mServerId, nRoleRequest.mServerId);
            }
            else
            {
                RoleAspect.insertRoleStart(nRoleRequest.mOperatorName, nRoleRequest.mVersionNo, accountInfo_.mAccountId, nRoleRequest.mServerId, nRoleRequest.mServerId);
            }
            return toJson(nRoleRequest);
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
