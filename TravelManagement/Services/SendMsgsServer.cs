using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using TravelManagement.Utilities;

namespace TravelManagement.Services
{
    public class SendMsgsServer
    {
        /// <summary>
        /// 推送咨询：通过openfire发送app广播
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public bool SendMsg(string content)
        {
            bool r= SendMsgHelper.Instance.SendMsg(content);
            if(r)
                Global.LogServer.SaveServerLogs("向所有用户推送消息，操作成功");
            else
            {
                Global.LogServer.SaveServerLogs("向所有用户推送消息，操作失败");
            }
            return r;
        }
    }
}
