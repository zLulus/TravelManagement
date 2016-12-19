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
    public class EmergencyCallServer
    {
        internal EmergencyCall MyEmergencyCall;
        /// <summary>
        /// 注意url参数不要有空格
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        public async Task<List<EmergencyCallModel>> GetSingleEmergencyCalls(DateTime startTime, DateTime endTime)
        {
            string start = startTime.ToString().Split(' ').FirstOrDefault().Replace("/", "-");    //只要年月日
            string end = endTime.ToString().Split(' ').FirstOrDefault().Replace("/", "-");
            string url = Global.url + "/GetEmergencyCalls/" + start + "/" + end;
            InternetHelperForList<EmergencyCallModel> helper2 = new InternetHelperForList<EmergencyCallModel>();
            var r =await helper2.GetList(url);
            return r;
        }

        public async void Statistics()
        {
            List<EmergencyCallModel> emergencyCalls =await GetSingleEmergencyCalls(MyEmergencyCall.StartTime,
                MyEmergencyCall.EndTime);
            MyEmergencyCall.EmergencyCallCollection=new ObservableCollection<EmergencyCallModel>(emergencyCalls);
        }

        public EmergencyCall GetEmergencyCall()
        {
            MyEmergencyCall=new EmergencyCall();
            return MyEmergencyCall;
        }
    }
}
