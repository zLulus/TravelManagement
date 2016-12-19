using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Layers;
using Esri.ArcGISRuntime.Symbology;
using TravelManagement.Helpers;
using TravelManagement.Models;

namespace TravelManagement.Services
{
    internal class SceniceSpotServiceAgent : ISceniceSpot,IRenderGraphic
    {
        public bool AlterScenicSpot(Scenic scenic)
        {
            var helper = new InternetHelperForList<Scenic>();
            bool r= helper.PostString(Global.url + "/AlterScenic", scenic);
            if(r)
                Global.LogServer.SaveServerLogs("修改"+scenic.ID+":"+scenic.Name+"景点信息成功");
            else
            {
                Global.LogServer.SaveServerLogs("修改" + scenic.ID + ":" + scenic.Name + "景点信息失败");
            }
            return r;
        }

        public async Task<bool> AddScenicSpot(Scenic scenic)
        {
            var helper = new InternetHelperForList<Scenic>();
            bool r= helper.PostString(Global.url + "/AddScenic", scenic);
            if (r)
                Global.LogServer.SaveServerLogs("新增景点"+scenic.Name+"成功");
            else
            {
                Global.LogServer.SaveServerLogs("新增景点" + scenic.Name + "失败");
            }
            return r;
        }

        public async Task<bool> DeleteScenicSpot(string id)
        {
            string url = Global.url + "/DeleteScenic/" + id;
            string result = await InternetHepler.Instance.UrlGetAsync(url);
            bool r= result.Contains("true");
            if (r)
                Global.LogServer.SaveServerLogs("删除景点" + id + "成功");
            else
            {
                Global.LogServer.SaveServerLogs("删除景点" + id + "失败");
            }
            return r;
        }

        public async Task<List<Scenic>> GetScenicSpotInfoList()
        {
            var helper = new InternetHelperForList<Scenic>();
            return await helper.GetList(Global.url + "/GetScenicInfoList");
        }

        public Graphic CreateGraphic<T, t>(int index, t symbol, T item) where t : Symbol
        {
            var itemScenic = item is Scenic ? item as Scenic : null;
            if (itemScenic == null)
                return new Graphic();
            var graphic = new Graphic
            {
                Geometry =
                    new MapPoint(Convert.ToDouble(itemScenic.longitude), Convert.ToDouble(itemScenic.latitude),
                        SpatialReferences.Wgs84),
                Symbol = symbol
            };
            graphic.Attributes["ID"] = itemScenic.id;
            graphic.Attributes["Name"] = itemScenic.name;
            graphic.Attributes["Description"] = itemScenic.description;
            graphic.Attributes["Longitude"] = itemScenic.longitude;
            graphic.Attributes["Latitude"] = itemScenic.latitude;
            return graphic;
        }

        public T CreatSymbol<T>() where T : Symbol
        {
            var symbol = new CompositeSymbol();
            symbol.Symbols.Add(new SimpleMarkerSymbol {Style = SimpleMarkerStyle.Circle, Color = Colors.Red, Size = 17});
            symbol.Symbols.Add(new TextSymbol
            {
                Text = "景", //中文支持有问题 解决方案：设置字体即可
                Font = new SymbolFont("宋体", 15),
                Color = Colors.White,
                VerticalTextAlignment = VerticalTextAlignment.Middle,
                HorizontalTextAlignment = HorizontalTextAlignment.Center,
                YOffset = -1
            });
            return symbol as T;
        }

        public T CreatSymbol<T>(Uri uri) where T : Symbol
        {
            return null;  //未用
        }
    }
}
