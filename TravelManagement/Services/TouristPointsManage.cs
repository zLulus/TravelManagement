using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Layers;
using Esri.ArcGISRuntime.Symbology;
using ManagerClientDemo.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TravelManagement.Helpers;
using TravelManagement.Models;

namespace TravelManagement.Services
{
    internal class TouristPointsManage : ITouristPoints,IRenderGraphic
    {
       
        /// <summary>
        ///     获取符合条件的游客们的当前位置（根据手机号/姓名）
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public List<Tourist> GetTouristLocation(string filter, List<Tourist> AllTouristCollection)
        {
            List<Tourist> result=new List<Tourist>();
            foreach (var tourist in AllTouristCollection)
            {
                try   //防空
                {
                    if (tourist.User.PhoneNum.Contains(filter) || tourist.User.Name.Contains(filter))
                        result.Add(tourist);
                }
                catch (Exception)
                {
                    
                }
                
            }
            return result;
            //var url = Global.url + "/GetTouristLocation/" + filter;
            //var helper = new InternetHelperForList<Tourist>();
            //return await helper.GetList(url);
        }

        /// <summary>
        ///     获取符合条件的游客们的当天行迹（根据手机号/姓名）
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public async Task<List<Schedule>> GetTouristsSchedules(string filter, DateTime startTime, DateTime endTime)
        {
            var start = startTime.ToString().Split(' ').FirstOrDefault().Replace("/", "-"); //只要年月日,2016/6/17->2016-6-17
            var end = endTime.ToString().Split(' ').FirstOrDefault().Replace("/", "-");
            var url = Global.url + "/GetTouristsSchedules/" + filter + "/" + start + "/" + end;
            var helper = new InternetHelperForList<Schedule>();
            return await helper.GetList(url);
        }

        /// <summary>
        ///     获取景区所有用户位置
        /// </summary>
        /// <returns></returns>
        public async Task<List<Tourist>> GetTouristsLocations()
        {
            var url = Global.url + "/GetTouristsLocations";
            var helper = new InternetHelperForList<Tourist>();
            return await helper.GetList(url);
        }

        

        public Graphic CreateGraphic<T, t>(int index, t symbol, T item) where t : Symbol
        {
            var itemScenic = item is Tourist ? item as Tourist : null;
            if (itemScenic == null)
                return new Graphic();
            var point = itemScenic.Point;
            var user = itemScenic.User;
            var graphic = new Graphic
            {
                Geometry =
                    new MapPoint(Convert.ToDouble(point.longitude), Convert.ToDouble(point.latitude),
                        SpatialReferences.Wgs84),
                Symbol = symbol
            };
            if (user != null)
            {
                graphic.Attributes["ID"] = user.PhoneNum;
                graphic.Attributes["Name"] = user.Name;
                graphic.Attributes["PhoneNum"] = user.PhoneNum;
                graphic.Attributes["Longitude"] = point.longitude;
                graphic.Attributes["Latitude"] = point.latitude;
            }
            return graphic;
        }

        public T CreatSymbol<T>(Uri uri) where T : Symbol
        {
            const int size = 36;
            var xPictureSymbol = new PictureMarkerSymbol {Width = size, Height = size, XOffset = 0, YOffset = 0};
            xPictureSymbol.SetSourceAsync(uri);
            return xPictureSymbol as T;
        }

        public T CreatSymbol<T>() where T : Symbol
        {
            return null; //未用
        }
    }
}
