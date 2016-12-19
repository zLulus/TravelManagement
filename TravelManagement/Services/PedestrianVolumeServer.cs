using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelManagement.Helpers;
using SimpleMvvmToolkit;
using TravelManagement.Models;
using System.Collections.ObjectModel;
using Newtonsoft.Json.Linq;

namespace TravelManagement.Services
{
    enum StatisticsWay
    {
        Day,
        Month
    }

    /// <summary>
    /// 人流量统计
    /// </summary>
    public class PedestrianVolumeServer
    {
        internal PedestrianVolume Pedestrian;

        public PedestrianVolumeServer()
        {
            Pedestrian=new PedestrianVolume();
        }

        public PedestrianVolume ReturnPedestrianVolume()
        {
            return Pedestrian;
        }

        public void Statistics()
        {
            if (Pedestrian.SelectedItem.Content.ToString() == "整月")
                StatisticsOneMonth();
            else
                StatisticsOneDay();
        }

        /// <summary>
        /// 统计某月人流量
        /// 每天一次
        /// </summary>
        private void StatisticsOneMonth()
        {
            StatisticsDetails(30, StatisticsWay.Month);
        }

        /// <summary>
        /// 统计某天人流量
        /// 每2h一次
        /// </summary>
        private void StatisticsOneDay()
        {
            StatisticsDetails(12, StatisticsWay.Day);
        }

        /// <summary>
        /// 统计具体方法抽象
        /// </summary>
        /// <param name="number"></param>
        /// <param name="way"></param>
        private async void StatisticsDetails(int number, StatisticsWay way)
        {
            Pedestrian.PedestrianVolumes.Clear();
            DateTime startTime=new DateTime();
            DateTime endTime=new DateTime();
            if (way == StatisticsWay.Day)    //整天
            {
                startTime = Pedestrian.StartTime;
                endTime = Pedestrian.StartTime.AddHours(2);
            }
            else    //整月
            {
                startTime = Pedestrian.StartTime.AddDays(-29);
                endTime = Pedestrian.StartTime.AddDays(-28);
            }
            for (int i = 0; i < number; i++)
            {
                int r = await StatisticsPedestrianVolume(startTime, endTime);
                Pedestrian.PedestrianVolumes.Add(new PedestrianVolumeModel()
                {
                    StartTime = startTime,
                    EndTime = endTime,
                    Number =r
                });
                if (way == StatisticsWay.Day)
                {
                    startTime = startTime.AddHours(2);
                    endTime = endTime.AddHours(2);
                }
                else
                {
                    startTime = startTime.AddDays(1);
                    endTime = endTime.AddDays(1);
                }
            }
        }

        /// <summary>
        /// 统计固定时间人流量
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        private async Task<int> StatisticsPedestrianVolume(DateTime startTime,DateTime endTime)
        {
            JObject jObject=new JObject();
            jObject.Add("startTime", startTime.ToString());
            jObject.Add("endTime", endTime.ToString());
            string r=await InternetHepler.Instance.PostJObject(jObject, "StatisticsPedestrianVolume");
            return int.Parse(r);
        }

        

        
    }
}
