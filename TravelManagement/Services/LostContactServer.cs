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
    public class LostContactServer
    {
        private LostContact lostContact;

        public LostContactServer()
        {
            lostContact=new LostContact();
        }

        public async void Statistics()
        {
            List<LostContactModel> singleLostContacts =await GetLostContacts(lostContact.StartTime, lostContact.EndTime,
                lostContact.HasSolved);
            lostContact.LostContactCollection=new ObservableCollection<LostContactModel>(singleLostContacts);
        }


        /// <summary>
        /// 获取异常情况记录
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="hasSolved">是否包含已解决的记录</param>
        public async Task<List<LostContactModel>> GetLostContacts(DateTime startTime, DateTime endTime, bool hasSolved)
        {
            string start = startTime.ToString().Split(' ').FirstOrDefault().Replace("/", "-");    //只要年月日
            string end = endTime.ToString().Split(' ').FirstOrDefault().Replace("/", "-");
            //12-15号
            InternetHelperForList<LostContactModel> helper2 = new InternetHelperForList<LostContactModel>();
            var r=await helper2.GetList(Global.url + "/GetLostContacts/" + start + "/" + end + "/" + hasSolved);
            return r;
        }

        public LostContact GetLostContact()
        {
            return lostContact;
        }

        /// <summary>
        /// 标记已解决
        /// </summary>
        /// <returns></returns>
        public async Task<bool> RecordSolved()
        {
            string url = Global.url + "/RecordSolved/" + lostContact.Selected.ID;
            string r= await InternetHepler.Instance.UrlGetAsync(url);
            return r.Contains("true");
        }
    }
}
