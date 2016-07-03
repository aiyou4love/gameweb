using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace gameweb.Controllers
{
    public class AccountController : ApiController
    {
        //http://localhost:8313/api/values/accountEnter
        //content-type: application/json;charset=utf-8
        //{"mAccountName": "zyh", "mAccountPassword": "123456", "mAgentName": "iosfigus", "mVersionNo": "1","mAccountId": "1", "mServerId": "1", "mPlayerId": "1", "mStart": "true"}
        [HttpPost]
        public HttpResponseMessage accountEnter([FromBody]EnterRequest nEnterRequest)
        {
            long accountId_ = AccountAspect.getAccountId(nEnterRequest.mAccountName, nEnterRequest.mPassword, nEnterRequest.mAccountType);
            EnterResult enterResult_ = new EnterResult();
            enterResult_.mSessionId = nEnterRequest.mSessionId;
            enterResult_.mErrorCode = ConstAspect.mFail;
            enterResult_.mAccountId = accountId_;
            enterResult_.mPlayerItem = null;
            if ( (accountId_ <= 0) || (accountId_ != nEnterRequest.mAccountId) )
            {
                enterResult_.mErrorCode = ConstAspect.mAccount;
                return toJson(enterResult_);
            }
            PlayerItem playerItem_ = PlayerAspect.getPlayerInfo(nEnterRequest.mOperatorName, nEnterRequest.mVersionNo, nEnterRequest.mAccountId, nEnterRequest.mPlayerId, nEnterRequest.mServerId);
            if (null == playerItem_)
            {
                enterResult_.mErrorCode = ConstAspect.mPlayer;
                return toJson(enterResult_);
            }
            enterResult_.mErrorCode = ConstAspect.mSucess;
            enterResult_.mPlayerItem = playerItem_;
            if (nEnterRequest.mStart)
            {
                PlayerAspect.updatePlayerStart(nEnterRequest.mOperatorName, nEnterRequest.mVersionNo, nEnterRequest.mAccountId, nEnterRequest.mServerId, nEnterRequest.mPlayerId);
            }
            return toJson(enterResult_);
        }

        //http://localhost:8313/api/values/accountLogin
        //content-type: application/json;charset=utf-8
        //{"mAccountName": "zyh", "mPassword": "123456", "mAgentName": "iosfigus", "mVersionNo": "1"}
        [HttpPost]
        public HttpResponseMessage accountLogin([FromBody]LoginRequest nLoginRequest)
        {
            long accountId_ = AccountAspect.getAccountId(nLoginRequest.mAccountName, nLoginRequest.mPassword, nLoginRequest.mAccountType);
            LoginResult loginResult_ = new LoginResult();
            loginResult_.mAccountId = accountId_;
            loginResult_.mPlayerItem = null;
            loginResult_.mServerItem = null;
            if (accountId_ > 0)
            {
                int serverId_ = 0;
                PlayerStart playerStart_ = PlayerAspect.getPlayerStart(nLoginRequest.mOperatorName, nLoginRequest.mVersionNo, accountId_);
                if (null != playerStart_)
                {
                    loginResult_.mPlayerItem = PlayerAspect.getPlayerInfo(nLoginRequest.mOperatorName, nLoginRequest.mVersionNo, accountId_, playerStart_.mPlayerId, playerStart_.mServerId);
                    serverId_ = playerStart_.mServerId;
                }
                else
                {
                    serverId_ = ServerAspect.getServerId(nLoginRequest.mOperatorName, nLoginRequest.mVersionNo);
                }
                loginResult_.mServerItem = ServerAspect.getServerItem(nLoginRequest.mOperatorName, nLoginRequest.mVersionNo, serverId_);
            }
            return toJson(loginResult_);
        }

        //http://localhost:8313/api/account/accountServers
        //content-type: application/json;charset=utf-8
        //{"mOperatorName": "iosfigus", "mVersionNo": "1", "mAccountId": "1"}
        [HttpPost]
        public HttpResponseMessage getServerList([FromBody]ServerRequest nServerRequest)
        {
            ServerResult serverResult_ = new ServerResult();
            serverResult_.mPlayerList = PlayerAspect.getPlayerList(nServerRequest.mOperatorName, nServerRequest.mVersionNo, nServerRequest.mAccountId);
            serverResult_.mServerList = ServerAspect.getServerList(nServerRequest.mOperatorName, nServerRequest.mVersionNo);
            return toJson(serverResult_);
        }

        //http://localhost:8313/api/account/accountRegister
        //content-type: application/json;charset=utf-8
        //{"mAccountName": "zyh", "mAccountPassword": "123456"}
        [HttpPost]
        public HttpResponseMessage accountRegister([FromBody]RegisterRequest nRegisterRequest)
        {
            if (AccountAspect.accountCheck(nRegisterRequest.mAccountName)) return toJson(false);

            return toJson(AccountAspect.accountRegister(nRegisterRequest.mAccountName, nRegisterRequest.mAccountPassword, 1));
        }

        //http://localhost:8313/api/account/accountCheck/?nAccountName=zyh
        [HttpGet]
        public HttpResponseMessage accountCheck(string nAccountName)
        {
            return toJson(AccountAspect.accountCheck(nAccountName));
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
