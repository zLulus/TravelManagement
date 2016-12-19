using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelManagement.Helpers;
using TravelManagement.Models;

namespace TravelManagement.Services
{
    public class AddAdminServer
    {
        public bool AddAdmin(Administrator Admin)
        {
            var helper = new InternetHelperForList<Administrator>();
            bool r= helper.PostString(Global.url + "/AddAdministrator", Admin);
            if (r)
            {
                Global.LogServer.SaveServerLogs("新增管理员" + Admin.account + "（账号）成功");
            }
            else
            {
                Global.LogServer.SaveServerLogs("新增管理员" + Admin.account + "（账号）失败");
            }
            return r;
        }
    }
}
