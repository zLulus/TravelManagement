using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Esri.ArcGISRuntime.Controls;
using Esri.ArcGISRuntime.Data;
using Esri.ArcGISRuntime.Layers;
using Esri.ArcGISRuntime.LocalServices;
using Newtonsoft.Json;

namespace TravelManagement.Services
{
    internal class LocalMapServiceAgent : IMapService
    {
        /// <summary>
        /// 创建本地要素服务，并加载底图
        /// </summary>
        /// <param name="myMapView"></param>
        public async void CreateLocalFeatureService(MapView myMapView)
        {
            try
            {
                //读取配置文件路径
                var localFeatureLocation = ConfigurationSettings.AppSettings["LocalFeatureLocation"];
                var featureLayersCount = Convert.ToInt32(ConfigurationSettings.AppSettings["FeatureLayersCount"]);
                //启动本地要素服务
                var localFeatureService = new LocalFeatureService(localFeatureLocation);
                await localFeatureService.StartAsync();
                //保存LocalFeatureUrl
                Global.LocalFeatureUrl = localFeatureService.UrlFeatureService;
                //加载Layers
                for (var i = 0; i < featureLayersCount; i++)
                {
                    var featureTable = new ServiceFeatureTable
                    {
                        ServiceUri = localFeatureService.UrlFeatureService + string.Format("/{0}", i)
                    };
                    await featureTable.InitializeAsync(myMapView.SpatialReference);
                    if (featureTable.IsInitialized)
                    {
                        var flayer = new FeatureLayer
                        {
                            ID = featureTable.Name, //table.Name,
                            FeatureTable = featureTable,
                            DisplayName = featureTable.Name
                        };
                        myMapView.Map.Layers.Add(flayer);
                        var extent = featureTable.ServiceInfo.Extent;
                        await myMapView.SetViewAsync(extent.Expand(1.10));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        /// <summary>
        /// 加载本地Map服务，并加载底图
        /// </summary>
        /// <param name="myMapView"></param>
        public async void CreatLocalMapService(MapView myMapView)
        {
            try
            {
                var localMapServiceUri = ConfigurationSettings.AppSettings["LocalMapServiceUri"];
                var localMapService = new LocalMapService(localMapServiceUri);
                await localMapService.StartAsync();

                var arcGISDynamicMapServiceLayer = new ArcGISDynamicMapServiceLayer
                {
                    ID = "arcGISDynamicMapServiceLayer",
                    DisplayName = "测试图层",
                    ServiceUri = localMapService.UrlMapService
                };
                myMapView.Map.Layers.Add(arcGISDynamicMapServiceLayer);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private async Task<string> DownLoadFeatureLayersCount(string Uri)
        {
            var data = "";
            HttpWebResponse response = null;
            try
            {
                var request = (HttpWebRequest) WebRequest.Create(Uri);
                request.Method = "GET";
                request.ContentType = "application/xml;charset=UTF-8";
                request.Accept =
                    "application/x-ms-application, image/jpeg, application/xaml+xml, image/gif, image/pjpeg, application/x-ms-xbap, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, */*";
                //request.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.1; WOW64; Trident/5.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; .NET4.0C; .NET4.0E)";
                response = (HttpWebResponse) request.GetResponse();
                //response采用"utf-8"进行编码的
                using (var reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")))
                {
                    data = reader.ReadToEnd();
                }
                return data;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                }
            }



        }
    }
}
