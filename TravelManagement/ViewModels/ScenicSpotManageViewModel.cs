using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Esri.ArcGISRuntime.Controls;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Layers;
using Esri.ArcGISRuntime.Symbology;
using FirstFloor.ModernUI.Windows.Controls;
using SimpleMvvmToolkit;
using TravelManagement.Models;
using TravelManagement.Services;
using TravelManagement.Views.Dialog;
using Point = System.Windows.Point;

// Toolkit namespace

namespace TravelManagement.ViewModels
{
    /// <summary>
    ///     This class contains properties that a View can data bind to.
    ///     <para>
    ///         Use the <strong>mvvmprop</strong> snippet to add bindable properties to this ViewModel.
    ///     </para>
    /// </summary>
    /// 该ViewModel的主要作用为景点管理
    public class ScenicSpotManageViewModel : ViewModelBase<ScenicSpotManageViewModel>
    {
        #region  fileds

        private MapView MyMapView;
        private Border mapTip;
        private GraphicsLayer _graphicsLayer;
        private bool isFirst = true;
        private bool _isHitTesting;
        private bool isDelete;
        private bool isAdd;
        private bool isAlter;
        private bool isEnable;
        //private bool addEnable;
        //private bool alterEnable;
        private const int MAX_GRAPHICS = 50;
        private readonly ISceniceSpot sceniceAgent;
        private ModernProgressRing progressRing;
        private bool isVisible;
        private Graphic graphic = new Graphic();

        public bool IsEnable
        {
            get { return isEnable; }
            set
            {
                isEnable = value;
                NotifyPropertyChanged(m => m.IsEnable);
            }
        }

        //public bool AddEnable
        //{
        //    get { return addEnable; }
        //    set
        //    {
        //        addEnable = value;
        //        NotifyPropertyChanged(m => m.AddEnable);
        //    }
        //}

        //public bool AlterEnable
        //{
        //    get { return alterEnable; }
        //    set
        //    {
        //        alterEnable = value;
        //        NotifyPropertyChanged(m => m.AlterEnable);
        //    }
        //}

        public Graphic Graphic
        {
            get { return graphic; }
            set
            {
                graphic = value;
                NotifyPropertyChanged(m => m.Graphic);
            }
        }
        public bool IsVisible
        {
            get { return isVisible; }
            set
            {
                isVisible = value;
                NotifyPropertyChanged(m => m.IsVisible);
            }
        }

        #endregion

        #region  Methods

        public ScenicSpotManageViewModel(ISceniceSpot sceniceAgent)
        {
            this.sceniceAgent = sceniceAgent;
        }

        public async void MyMapView_OnMouseMove(object sender, MouseEventArgs e)
        {
            if (_isHitTesting)
                return;
            try
            {
                _isHitTesting = true;

                var screenPoint = e.GetPosition(MyMapView);
                var graphic = await _graphicsLayer.HitTestAsync(MyMapView, screenPoint);
                if (graphic != null)
                {
                    Graphic = graphic;
                    IsVisible = true;
                }
                else
                    IsVisible = false;
            }
            catch
            {
                IsVisible = false;
            }
            finally
            {
                _isHitTesting = false;
            }
        }

        public async void MyMapView_OnMapViewTapped(object sender, MapViewInputEventArgs e)
        {
            try
            {
                var normalizedPoint = GeometryEngine.NormalizeCentralMeridian(e.Location);
                var point = GeometryEngine.Project(normalizedPoint, SpatialReferences.Wgs84) as MapPoint;

                if (isDelete)
                {
                    await DeleteScenic(e.Position);
                    //return;
                }
                if (isAdd)
                {
                    await AddScenic(point);
                    //return;
                }
                if (isAlter)
                {
                    await AlterScenic(e.Position);
                    //return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("HitTest Error: " + ex.Message, "Graphics Layer Hit Testing");
            }
        }

        private async Task AlterScenic(Point point)
        {
            var graphic = await _graphicsLayer.HitTestAsync(MyMapView, point, MAX_GRAPHICS);
            var _graphic = graphic.FirstOrDefault();
            var x = new Dialog_ScenicScan();
            x.Title = "修改景点";
            string id = _graphic.Attributes["ID"].ToString();
            x.TxtName.Text= _graphic.Attributes["Name"].ToString();
            x.TxtDescription.Text= _graphic.Attributes["Description"].ToString();
            //也可以采用点击位置的方式
            double longitude= double.Parse(_graphic.Attributes["Longitude"].ToString());
            double latitude = double.Parse(_graphic.Attributes["Latitude"].ToString());
            x.ShowDialog();
            string ScenicName = x.TxtName.Text;
            string ScenicDescription = x.TxtDescription.Text;
            if (ScenicName != string.Empty && ScenicDescription != string.Empty)
            {
                bool r=sceniceAgent.AlterScenicSpot(new Scenic()
                {
                    id = int.Parse(id),
                    description = ScenicDescription,
                    name = ScenicName,
                    longitude = (decimal)longitude,
                    latitude = (decimal)latitude
                });

                if (r)   //修改UI
                {
                    _graphic.Attributes["Name"] = ScenicName;
                    _graphic.Attributes["Description"] = ScenicDescription;
                    MessageBox.Show("修改景点信息成功", "操作成功", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("修改景点信息失败", "操作失败", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private async Task AddScenic(MapPoint location)
        {
            var x = new Dialog_ScenicScan();
            //x.Owner = Application.Current.MainWindow;
            x.ShowDialog();
            var ScenicName = x.TxtName.Text;
            var ScenicDescription = x.TxtDescription.Text;
            var graphic = new Graphic();
            //子窗体关闭按钮已做处理
            if (ScenicName != string.Empty && ScenicDescription != string.Empty)
            {
                var symbol = (sceniceAgent as IRenderGraphic).CreatSymbol<CompositeSymbol>();
                graphic = new Graphic
                {
                    Geometry = location,
                    Symbol = symbol
                };
                var item = new Scenic
                {
                    description = ScenicDescription,
                    longitude = (decimal?) location.X,
                    latitude = (decimal?) location.Y,
                    id = Convert.ToInt32(_graphicsLayer.Graphics.Last().Attributes["ID"]) + 1,
                    name = ScenicName
                };
                graphic.Attributes["ID"] = item.id;
                graphic.Attributes["Name"] = ScenicName;
                graphic.Attributes["Description"] = ScenicDescription;
                graphic.Attributes["Longitude"] =location.X;
                graphic.Attributes["Latitude"] =location.Y;
                _graphicsLayer.Graphics.Add(graphic);
                var result = await sceniceAgent.AddScenicSpot(item);
                if (result)   //新增成功
                {
                    MessageBox.Show("新增景点信息成功", "操作成功", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    _graphicsLayer.Graphics.Remove(graphic);
                    MessageBox.Show("新增景点信息失败", "操作失败", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private async Task<bool> DeleteScenic(Point point)
        {
            var graphic = await _graphicsLayer.HitTestAsync(MyMapView, point, MAX_GRAPHICS);
            if (isDelete)
            {
                if (graphic == null || graphic.Count() == 0)
                    return true;
                var r=MessageBox.Show("你确定要删除该景点信息吗", "消息提示", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (r==MessageBoxResult.Yes)
                {
                    var _graphic = graphic.FirstOrDefault();
                    var result = await sceniceAgent.DeleteScenicSpot(_graphic.Attributes["ID"].ToString());
                    if (result)   //删除成功
                    {
                        _graphicsLayer.Graphics.Remove(_graphic);
                        MessageBox.Show("删除景点信息成功", "操作成功", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("删除景点信息失败", "操作失败", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                //isDelete = false;
            }
            return false;
        }

        #region  Graphics

        private async void CreateGraphics()
        {
            var list = await sceniceAgent.GetScenicSpotInfoList();
            for (var n = 0; n < list.Count; ++n)
            {
                var item = list[n];
                var symbol = (sceniceAgent as IRenderGraphic).CreatSymbol<CompositeSymbol>();
                var grapic = (sceniceAgent as IRenderGraphic).CreateGraphic(n, symbol, item);
                _graphicsLayer.Graphics.Add(grapic);
            }
            IsEnable = true;
            var currentViewpoint = MyMapView.GetCurrentViewpoint(ViewpointType.BoundingGeometry);
            var viewpointExtent = currentViewpoint.TargetGeometry.Extent;
            await MyMapView.SetViewAsync(viewpointExtent.Expand(1.10));
        }

        #endregion

        #endregion

        #region Command

        public ICommand LoadScenicCommand
        {
            get
            {
                return new DelegateCommand<MapView>(mapView =>
                {
                    MyMapView = mapView;
                    MyMapView.Map.SpatialReference = SpatialReferences.Wgs84;
                    _graphicsLayer = MyMapView.Map.Layers["GraphicsLayer"] as GraphicsLayer;
                    IsEnable = false;
                    if (isFirst)
                    {
                        CreateGraphics();
                        isFirst = false;
                    }
                });
            }
        }

        public ICommand LoadMaptipCommand
        {
            get { return new DelegateCommand<Border>(maptip => mapTip = maptip); }
        }

        public ICommand AlterCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    isDelete = false;
                    isAdd = false;
                    isAlter = true;
                });
            }
        }

        public ICommand DeleteCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    isDelete = true;
                    isAdd = false;
                    isAlter = false;
                });
            }
        }

        public ICommand AddCommand
        {
            get
            {
                return new DelegateCommand<ModernProgressRing>(ring =>
                {
                    progressRing = ring;
                    isAdd = true;
                    isDelete = false;
                    isAlter = false;
                });
            }
        }

        #endregion
    }
}