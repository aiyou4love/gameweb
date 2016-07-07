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
    public class PlayerController : ApiController
    {
        //http://localhost:8313/api/values/createPlayer
        //content-type: application/json;charset=utf-8
        //{"mAccountName": "zyh", "mAccountPassword": "123456", "mAgentName": "iosfigus", "mVersionNo": "1","mAccountId": "1", "mServerId": "1", "mPlayerName": "赵由华", "nPlayerRace": "1", "nUpdate": "true"}
        [HttpPost]
        public HttpResponseMessage createPlayer([FromBody]PlayerRequest nPlayerRequest)
        {
            long accountId_ = AccountAspect.getAccountId(nPlayerRequest.mAccountName, nPlayerRequest.mPassword, nPlayerRequest.mAccountType);
            PlayerResult playerResult_ = new PlayerResult();
            playerResult_.mErrorCode = ConstAspect.mFail;
            playerResult_.mAccountId = accountId_;
            playerResult_.mPlayerItem = null;
            playerResult_.mServerItem = null;
            if ((0 == accountId_) || (nPlayerRequest.mAccountId != accountId_))
            {
                playerResult_.mErrorCode = ConstAspect.mAccount;
                return toJson(playerResult_);
            }
            int playerCount_ = PlayerAspect.getPlayerCount(nPlayerRequest.mOperatorName, nPlayerRequest.mVersionNo, accountId_, nPlayerRequest.mServerId);
            if (playerCount_ > 0)
            {
                playerResult_.mErrorCode = ConstAspect.mPlayer;
                return toJson(playerResult_);
            }
            if (PlayerAspect.createPlayer(nPlayerRequest.mOperatorName, nPlayerRequest.mVersionNo, accountId_, nPlayerRequest.mServerId, nPlayerRequest.mPlayerName, nPlayerRequest.mPlayerRace))
            {
                playerResult_.mErrorCode = ConstAspect.mSucess;
                playerResult_.mAccountId = accountId_;
                playerResult_.mPlayerItem = new PlayerItem();
                playerResult_.mPlayerItem.mPlayerId = nPlayerRequest.mServerId;
                playerResult_.mPlayerItem.mServerId = nPlayerRequest.mServerId;
                playerResult_.mPlayerItem.mPlayerName = nPlayerRequest.mPlayerName;
                playerResult_.mPlayerItem.mPlayerRace = nPlayerRequest.mPlayerRace;
                playerResult_.mPlayerItem.mPlayerStep = 1;
                playerResult_.mPlayerItem.mPlayerLevel = 1;
                playerResult_.mPlayerItem.mPlayerType = 1;
                playerResult_.mServerItem = ServerAspect.getServerItem(nPlayerRequest.mOperatorName, nPlayerRequest.mVersionNo, nPlayerRequest.mServerId);
            }
            else
            {
                playerResult_.mErrorCode = ConstAspect.mCreate;
            }
            if (nPlayerRequest.mUpdate)
            {
                PlayerAspect.updatePlayerStart(nPlayerRequest.mOperatorName, nPlayerRequest.mVersionNo, accountId_, nPlayerRequest.mServerId, nPlayerRequest.mServerId);
            }
            else
            {
                PlayerAspect.insertPlayerStart(nPlayerRequest.mOperatorName, nPlayerRequest.mVersionNo, accountId_, nPlayerRequest.mServerId, nPlayerRequest.mServerId);
            }
            return toJson(nPlayerRequest);
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
