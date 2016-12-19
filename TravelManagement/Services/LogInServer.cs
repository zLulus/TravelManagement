using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TravelManagement.Helpers;
using TravelManagement.Models;

namespace TravelManagement.Services
{
    public class LogInServer
    {
        public async Task<bool> LogIn(string account, string password)
        {
            JObject jObject=new JObject();
            jObject.Add("account", account);
            jObject.Add("password", password);
            string r=await InternetHepler.Instance.PostJObject(jObject, "LogInAdmin");
            if (string.IsNullOrEmpty(r))
                return false;
            try
            {
                Global.LogAdministrator = JsonConvert.DeserializeObject<Administrator>(r);
                Global.LogServer=new ServerLogServer();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            
        }
    }
}
