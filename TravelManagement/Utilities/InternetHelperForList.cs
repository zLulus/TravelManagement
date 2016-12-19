using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TravelManagement.Helpers
{
    public class InternetHelperForList<T> where T : class
    {
        /// <summary>
        /// POST方式，StringContent，返回bool型
        /// </summary>
        /// <param name="url"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool PostString(string url, T t)
        {
            //todo 改为异步
            string str = JsonConvert.SerializeObject(t);
            HttpContent content = new StringContent(str, Encoding.UTF8, "application/json");
            HttpClient client = new HttpClient();
            var r = client.PostAsync(url, content).Result.Content.ReadAsStringAsync().Result;
            if (r.Contains("true"))
                return true;
            return false;
        }

        /// <summary>
        /// GET方式 返回对象集合
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns> 
        public async Task<List<T>> GetList(string url)
        {
            try
            {
                string result = await InternetHepler.Instance.UrlGetAsync(url);
                List<T> list = JsonConvert.DeserializeObject<List<T>>(result);
                return list;
            }
            catch (Exception)
            {
                return new List<T>();
            }
        }
    }
}
