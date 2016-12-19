using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelManagement.Helpers;
using TravelManagement.Models;

namespace TravelManagement.Services
{
    public class ServerLogServer
    {
        /// <summary>
        /// 保存操作记录
        /// </summary>
        /// <param name="action">工作的内容+工作情况</param>
        public async void SaveServerLogs(string action)
        {
            string account = Global.LogAdministrator.Account;
            string url = Global.url + "/SaveServerLogs/" + "账号" + account + action;
            await InternetHepler.Instance.UrlGetAsync(url);
        }

        public async Task<ObservableCollection<ServerLog>> GetServerLogs(DateTime startTime, DateTime endTime)
        {
            string time1 = startTime.ToString().Split(' ').FirstOrDefault().Replace("/", "-");
            //AddDays 保证查询到当天的记录
            string time2 = endTime.AddDays(1).ToString().Split(' ').FirstOrDefault().Replace("/", "-");
            string url = Global.url + "/GetServerLogs/" + time1 + "/" + time2;
            InternetHelperForList<ServerLog> helper=new InternetHelperForList<ServerLog>();
            var r=await helper.GetList(url);
            return new ObservableCollection<ServerLog>(r);
        }
    }
}
