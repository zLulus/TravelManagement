using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Esri.ArcGISRuntime.Layers;
using Esri.ArcGISRuntime.Symbology;

namespace TravelManagement.Services
{
    interface IRenderGraphic
    {
        /// <summary>
        /// 创建Graphic
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="index"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        Graphic CreateGraphic<T,t>(int index,t symbol, T item)where t:Symbol;


        /// <summary>
        /// 创建Symbol
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>

        T CreatSymbol<T>(Uri uri) where T : Symbol;

        T CreatSymbol<T>() where T : Symbol;


    }
}
