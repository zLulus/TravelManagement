using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace TravelManagement.Helpers
{
    public class InternetHepler
    {
        private static InternetHepler _instance = null;
        private static readonly object lockHelper = new object();
        private InternetHepler() { }
        public static InternetHepler Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (lockHelper)
                    {
                        if (null == _instance)
                        {
                            _instance = new InternetHepler();
                        }
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// jObject POST 抽象方法
        /// </summary>
        /// <param name="jObject"></param>
        /// <param name="controller">url</param>
        /// <returns></returns>
        public async Task<string> PostJObject(JObject jObject, string controller)
        {
            WebClient c = new WebClient();
            c.Headers[HttpRequestHeader.ContentType] = "application/json";
            c.Encoding = System.Text.Encoding.UTF8; //定义对象的编码语言,此处或者是gb2312
            string result =await c.UploadStringTaskAsync(Global.url + "/" + controller, jObject.ToString(Newtonsoft.Json.Formatting.None, null));
            return result;
        }

        /// <summary>
        /// 异步 GET方式
        /// 返回的Task<string>不一定是Task<string>，如果是对象，Json反序列化
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<string> UrlGetAsync(string url)
        {
            try
            {
                HttpClient client = new HttpClient();
                var response = await client.GetAsync(url);
                string ds =await response.Content.ReadAsStringAsync();
                return ds;
            }
            catch (HttpRequestException x)
            {
                return "无法连接服务器.....";
            }
        }

        /// <summary>
        /// 同步 GET方式
        /// 返回的string不一定是string，如果是对象，Json反序列化
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public string UrlGet(string url)
        {
            try
            {
                WebClient c = new WebClient();
                c.Headers[HttpRequestHeader.ContentType] = "application/json";
                c.Encoding = System.Text.Encoding.UTF8;
                string s = c.DownloadString(url);
                return s;

                //todo why?
                //HttpClient client = new HttpClient();

                //string ds = client.GetAsync(url).Result.Content.ReadAsStringAsync().Result;
                //return ds;
            }
            catch (Exception x)
            {
                return "无法连接服务器.....";
            }
        }

    }
}
