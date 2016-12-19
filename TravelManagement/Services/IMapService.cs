using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Esri.ArcGISRuntime.Controls;

namespace TravelManagement.Services
{
    public interface IMapService
    {
        /// <summary>
        /// 创建本地要素服务,加载底图
        /// </summary>
        void CreateLocalFeatureService(MapView myMapView);

        /// <summary>
        /// 创建本地MapService，加载底图
        /// </summary>
        /// <param name="myMapView"></param>
        void CreatLocalMapService(MapView myMapView);

    }
}
