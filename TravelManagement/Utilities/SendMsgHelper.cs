using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace TravelManagement.Utilities
{
    public class SendMsgHelper
    {
        private static SendMsgHelper _instance = null;
        private static readonly object lockHelper = new object();
        public static SendMsgHelper Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (lockHelper)
                    {
                        if (null == _instance)
                        {
                            _instance = new SendMsgHelper();
                        }
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// 指定用户/所有用户发短信客户端
        /// </summary>
        /// <param name="templateID">模板ID（自己设置）</param>
        /// <param name="content">发送内容集合（填空）</param>
        /// <param name="phoneNumbers">填写则是指定用户发送短信，不填写则是所有用户发送短信</param>
        public bool SendEmergencyMsgs(string templateID, List<string> content, List<string> phoneNumbers = null)
        {
            string method = "";
            JObject json = new JObject(); //包装请求
            JToken jToken2 = new JArray(content);
            json.Add("content", jToken2);
            if (phoneNumbers != null)
            {
                JToken jToken = new JArray(phoneNumbers);
                json.Add("phoneNumbers", jToken);
                method = "SendMsgToTargetsByNote";
            }
            else
            {
                method = "SendMsgsByNote";
            }
            WebClient c = new WebClient();
            c.Headers[HttpRequestHeader.ContentType] = "application/json";
            c.Encoding = System.Text.Encoding.UTF8;
            string url = string.Format(Global.url+"/{0}/{1}", method, templateID);
            string result = c.UploadString(url, json.ToString(Newtonsoft.Json.Formatting.None, null));
            if (result.Contains("true"))
                return true;
            return false;
        }

        /// <summary>
        /// 推送咨询：通过openfire发送app广播
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public bool SendMsg(string content)
        {
            JObject json = new JObject(); //包装请求
            json.Add("content", content);
            WebClient c = new WebClient();
            c.Headers[HttpRequestHeader.ContentType] = "application/json";
            c.Encoding = System.Text.Encoding.UTF8;
            string result = c.UploadString(Global.url + "/SendMsgs",
                json.ToString(Newtonsoft.Json.Formatting.None, null));
            if (result.Contains("true"))
                return true;
            return false;
        }

        /// <summary>
        /// 指定用户群推送咨询：通过openfire发送app广播
        /// </summary>
        /// <param name="content"></param>
        /// <param name="phoneNumbers"></param>
        /// <returns></returns>
        public bool SendMsgToTargets(string content, List<string> phoneNumbers)
        {
            JObject json = new JObject(); //包装请求
            json.Add("content", content);
            JToken jToken = new JArray(phoneNumbers);
            json.Add("phoneNumbers", jToken);
            WebClient c = new WebClient();
            c.Headers[HttpRequestHeader.ContentType] = "application/json";
            c.Encoding = System.Text.Encoding.UTF8;
            string result = c.UploadString(Global.url + "/SendMsgToTargets",
                json.ToString(Newtonsoft.Json.Formatting.None, null));
            if (result.Contains("true"))
                return true;
            return false;
        }
    }
}
