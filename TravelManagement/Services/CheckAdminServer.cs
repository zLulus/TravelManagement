using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TravelManagement.Helpers;
using TravelManagement.Models;

namespace TravelManagement.Services
{
    public class CheckAdminServer
    {
        public async Task<ObservableCollection<Administrator>> GetAdministrators()
        {
            InternetHelperForList<Administrator> helper2 = new InternetHelperForList<Administrator>();
            var r = await helper2.GetList(Global.url + "/GetAdministrators");
            return new ObservableCollection<Administrator>(r);
            //string r = InternetHepler.Instance.UrlGet(Global.url + "/GetAdministrators");
            //var result=JsonConvert.DeserializeObject<ObservableCollection<Administrator>>(r);
            //return result;
        }

        public bool DeleteAdmin(Administrator admin)
        {
            if (admin != null)
            {
                string url = Global.url + "/DeleteAdministrator/" + admin.Account;
                string r=InternetHepler.Instance.UrlGet(url);
                if (r.Contains("true"))
                {
                    Global.LogServer.SaveServerLogs("删除管理员"+admin.account+"（账号）成功");
                    return true;
                }
            }
            Global.LogServer.SaveServerLogs("删除管理员" + admin.account + "（账号）失败");
            return false;
        }

        public async Task<bool> AlterPassword(string account, string newPassword)
        {
            JObject jObject=new JObject();
            jObject.Add("account", account);
            jObject.Add("newPassword", newPassword);
            string r=await InternetHepler.Instance.PostJObject(jObject, "AlterPassword");
            if (r.Contains("true"))
            {
                Global.LogServer.SaveServerLogs("删除管理员" + account + "（账号）密码成功");
                return true;
            }
            Global.LogServer.SaveServerLogs("删除管理员" + account + "（账号）密码失败");
            return false;
        }

        public async Task<bool> AlterInfo(string account, string name, string sex, string position)
        {
            string url = Global.url + "/AlterInfo/" + account + "/" + name + "/" + sex + "/" + position;
            string r= await InternetHepler.Instance.UrlGetAsync(url);
            if (r.Contains("true"))
            {
                Global.LogServer.SaveServerLogs("删除管理员" + account + "（账号）信息成功");
                return true;
            }
            Global.LogServer.SaveServerLogs("删除管理员" + account + "（账号）信息失败");
            return false;
        }
    }
}
