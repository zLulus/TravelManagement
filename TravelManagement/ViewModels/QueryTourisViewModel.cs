using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using Esri.ArcGISRuntime.Controls;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Layers;
using Esri.ArcGISRuntime.Symbology;
using ManagerClientDemo.Entities;
using SimpleMvvmToolkit;
using TravelManagement.Models;
using TravelManagement.Services;
using TravelManagement.Views.Dialog;
using Point = TravelManagement.Models.Point;

// Toolkit namespace

namespace TravelManagement.ViewModels
{
    /// <summary>
    ///     This class contains properties that a View can data bind to.
    ///     <para>
    ///         Use the <strong>mvvmprop</strong> snippet to add bindable properties to this ViewModel.
    ///     </para>
    /// </summary>
    public class QueryTourisViewModel : ViewModelDetailBase<QueryTourisViewModel,Tourist>
    {
        #region fileds

        private readonly ITouristPoints serviceAgent;
        private List<Tourist> touristCollection;
        private List<Tourist> allTouristCollection;
        private GraphicsLayer _graphicsLayer;
        private DataGrid grid;
        private bool isEnabled;

        public bool IsEnabled
        {
            get { return isEnabled; }
            set
            {
                isEnabled = value;
                NotifyPropertyChanged(m => m.IsEnabled);
            }
        }
        public List<Tourist> TouristCollection
        {
            get { return touristCollection; }
            set
            {
                touristCollection = value;
                NotifyPropertyChanged(m => m.TouristCollection);
            }
        }

        public List<Tourist> AllTouristCollection
        {
            get { return allTouristCollection; }
            set
            {
                allTouristCollection = value;
                NotifyPropertyChanged(m => m.AllTouristCollection);
            }
        }

        #endregion

        public QueryTourisViewModel(ITouristPoints serviceAgent)
        {
            IsEnabled = false;
            this.serviceAgent = serviceAgent;
            Task.Run((async () =>
            {
                TouristCollection = await serviceAgent.GetTouristsLocations();
                AllTouristCollection=new List<Tourist>(touristCollection);
                IsEnabled = true;
            }));
        }

        #region Methods

        public async void LocationButton_OnClick(object sender, RoutedEventArgs e)
        {
            var item = grid.SelectedItem as Tourist;
            var x = new Dialog_ShowTouristLocation();
            _graphicsLayer = x.MyMapView.Map.Layers["GraphicsLayer"] as GraphicsLayer;
            var symbol = (serviceAgent as IRenderGraphic).CreatSymbol<PictureMarkerSymbol>(new Uri(Global.LocalMarkerUrl));
            var graphic = (serviceAgent as IRenderGraphic).CreateGraphic(1, symbol, item);
            _graphicsLayer.Graphics.Add(graphic);
            x.Owner = Application.Current.MainWindow;
            double _x = (double)item.Point.longitude;
            double _y = (double)item.Point.latitude;
            var minPoint = new MapPoint(_x - 10, _y - 10, SpatialReferences.Wgs84);
            var maxPoint = new MapPoint(_x + 10, _y + 10, SpatialReferences.Wgs84);
            Envelope envelope = new Envelope(minPoint, maxPoint);
            x.ShowDialog();
            await
                x.MyMapView.SetViewAsync(envelope);
        }

        public async void TrackButton_OnClick(object sender, RoutedEventArgs e)
        {
            var item = grid.SelectedItem as Tourist;
            var dialog = new Dialog_ShowTouristLocation();
            dialog.MyMapView.Map.SpatialReference = SpatialReferences.Wgs84;
            _graphicsLayer = dialog.MyMapView.Map.Layers["GraphicsLayer"] as GraphicsLayer;
            //x.Owner = Application.Current.MainWindow;
            //当天(19)轨迹：查询时间为今天-明天(19-20)
            var list =await serviceAgent.GetTouristsSchedules(item.User.PhoneNum, DateTime.Now,DateTime.Now.AddDays(1));
            //foreach (var li in list)   //根据手机号查询，不会有多个行程结果
            //{
            double xMin=999, yMin=999, xMax=0, yMax=0;
            List<MapPoint> points=new List<MapPoint>();
            for (var i = 0; i < list[0].points.Count; i++)
            {
                double x = (double)list[0].points[i].longitude;
                double y = (double)list[0].points[i].latitude;
                points.Add(new MapPoint(x,y , SpatialReferences.Wgs84));
                if (x < xMin)
                    xMin = x;
                else if(x>xMax)
                {
                    xMax = x;
                }

                _graphicsLayer.Graphics.Add(CreateTrackGraphic(i, list[0].points[i], list[0]));
            }
            

            SimpleLineSymbol lineSymbol = new SimpleLineSymbol();
            lineSymbol.Color = Color.FromRgb(255, 100, 0);
            lineSymbol.Width = 5;
            Polyline line=new Polyline(points,SpatialReferences.Wgs84);
            //var line = await dialog.MyMapView.Editor.RequestShapeAsync(DrawShape.Polyline,lineSymbol, null);
            Graphic graphic=new Graphic(line, lineSymbol);
            
            _graphicsLayer.Graphics.Add(graphic);
            //}
            var point = list.Last().points.Last();
            dialog.MyMapView.Map.InitialViewpoint =
                new Viewpoint(new MapPoint((double) point.longitude, (double) point.latitude));
            dialog.ShowDialog();
        }

        private Graphic CreateTrackGraphic(int i, Point point, Schedule li)
        {
            var symbol = (serviceAgent as IRenderGraphic).CreatSymbol<PictureMarkerSymbol>();
            var graphic = new Graphic
            {
                Geometry =
                    new MapPoint(Convert.ToDouble(point.longitude), Convert.ToDouble(point.latitude),
                        SpatialReferences.Wgs84),
                Symbol = symbol
            };
            graphic.Attributes["PhoneNum"] = "手机号: " +li.user.PhoneNum;
            graphic.Attributes["Name"] = "姓名: " + point.longitude;
            graphic.Attributes["Longitude"] = "经度: " + point.longitude;
            graphic.Attributes["Latitude"] = "纬度: " + point.latitude;
            graphic.Attributes["Time"] = "时间: " + point.time;
            return graphic;
        }
        #endregion

        #region Command

        public ICommand LoadCommand
        {
            get { return new DelegateCommand<DataGrid>((grid => { this.grid = grid; })); }
        }

        public ICommand SearchCommand
        {
            get
            {
                return new DelegateCommand<string>(tvSearchContent =>
                {
                    if (tvSearchContent.Trim() != string.Empty)
                    {
                        TouristCollection = serviceAgent.GetTouristLocation(tvSearchContent,AllTouristCollection);
                    }
                    else
                    {
                        MessageBox.Show("请输入正确的查询条件");
                    }
                });
            }
        }

        #endregion
    }
}