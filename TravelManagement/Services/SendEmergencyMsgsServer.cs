using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using TravelManagement.Utilities;

namespace TravelManagement.Services
{
    public class SendEmergencyMsgsServer
    {
        /// <summary>
        /// 所有用户发短信客户端
        /// </summary>
        /// <param name="templateID">模板ID（自己设置）</param>
        /// <param name="content">发送内容集合（填空）</param>
        /// <param name="phoneNumbers">填写则是指定用户发送短信，不填写则是所有用户发送短信</param>
        public bool SendEmergencyMsgs(string templateID, List<string> content, List<string> phoneNumbers = null)
        {
            bool r= SendMsgHelper.Instance.SendEmergencyMsgs(templateID, content, phoneNumbers);
            if (r)
                Global.LogServer.SaveServerLogs("向所有用户推送紧急短信，操作成功");
            else
            {
                Global.LogServer.SaveServerLogs("向所有用户推送紧急短信，操作失败");
            }
            return r;
        }
    }
}
