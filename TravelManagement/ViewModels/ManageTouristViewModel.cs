using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
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
using Esri.ArcGISRuntime.Symbology.SceneSymbology;
using Esri.ArcGISRuntime.Tasks.Query;
using FirstFloor.ModernUI.Presentation;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SimpleMvvmToolkit;
using TravelManagement.Helpers;
using TravelManagement.Models;
using TravelManagement.Services;
using TravelManagement.Utilities;

// Toolkit namespace

namespace TravelManagement.ViewModels
{
    /// <summary>
    ///     This class contains properties that a View can data bind to.
    ///     <para>
    ///         Use the <strong>mvvmprop</strong> snippet to add bindable properties to this ViewModel.
    ///     </para>
    /// </summary>
    public class ManageTouristViewModel : ViewModelBase<ManageTouristViewModel>
    {
        #region  fileds

        private Border mapTip;
        private MapView MyMapView;
        private MapView overViewMap;
        private Graphic graphic = new Graphic();
        private GraphicsLayer _graphicsLayer;
        private GraphicsOverlay overviewOverlay;
        private readonly ITouristPoints touristAgent;
        private bool isFirst = true;
        private bool _isHitTesting;
        private bool isVisible;
       

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

        #region method

        public async void MyMapView_OnMouseMove(object sender, MouseEventArgs e)
        {
            if (_isHitTesting)
                return;
            try
            {
                _isHitTesting = true;

                var screenPoint = e.GetPosition(MyMapView);
                graphic = await _graphicsLayer.HitTestAsync(MyMapView, screenPoint);
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


        public ManageTouristViewModel(ITouristPoints serviceAgent)
        {
            touristAgent = serviceAgent;
            SelectText = "选择范围";
            SelectedIndex = 0;
            SetSendText();
            isSendMsgsVisible = false;
            IsSendEmergencyMsgsVisible = false;
            isEdit = false;
            SearchTime = string.Format("{0:F}", DateTime.Now);
            SearchBtIsEnabled = true;
        }


        private async void CreateGraphics()
        {
            TouristsList = await touristAgent.GetTouristsLocations();
            double xMin = 999;
            double yMin = 999;
            double xMax = 0;
            double yMax = 0;
            for (var n = 0; n < TouristsList.Count; ++n)
            {
                var item = TouristsList[n];
                var symbol = (touristAgent as IRenderGraphic).CreatSymbol<PictureMarkerSymbol>(new Uri(Global.LocalMarkerUrl));
                var graphic = (touristAgent as IRenderGraphic).CreateGraphic(n, symbol, item);
                double x = (double)item.Point.longitude;
                double y = (double)item.Point.latitude;
                if (x < xMin)
                    xMin = x;
                else if(x>xMax)
                {
                    xMax = x;
                }
                if (y < yMin)
                    yMin = y;
                else if(y>yMax)
                {
                    yMax = y;
                }
                _graphicsLayer.Graphics.Add(graphic);
            }
            var  minPoint=new MapPoint(xMin,yMin,SpatialReferences.Wgs84);
            var maxPoint = new MapPoint(xMax, yMax, SpatialReferences.Wgs84);
            Envelope envelope = new Envelope(minPoint, maxPoint);//xMin, yMin, xMax, yMax
            //var currentViewpoint = MyMapView.GetCurrentViewpoint(ViewpointType.BoundingGeometry);
            //var viewpointExtent = currentViewpoint.TargetGeometry.Extent;
            await MyMapView.SetViewAsync(envelope.Expand(1.10));
        }

        private async void LoadOverView()
        {
            try
            {
                await MyMapView.LayersLoadedAsync();
                overviewOverlay = overViewMap.GraphicsOverlays["overviewOverlay"];
                while (true)
                {
                    var symbol = overViewMap.Resources["RedFillSymbol"] as Symbol;
                    var geometry = await overViewMap.Editor.RequestShapeAsync(DrawShape.Rectangle, symbol);
                    overviewOverlay.Graphics.Clear();

                    var graphic = new Graphic(geometry);
                    overviewOverlay.Graphics.Add(graphic);
                    var viewpointExtent = geometry.Extent;
                    await
                        MyMapView.SetViewAsync(viewpointExtent);
                }
            }
            catch (Exception)
            {

            }
        }

        #endregion


        #region ZL

        private List<Tourist> TouristsList;
        private string selectText;

        private string searchTime;
        //发送用户点查看请求的终止时间，起始时间为searchTime_end-1h
        private DateTime searchTime_end;

        /// <summary>
        /// 游客点查看时刻
        /// </summary>
        public string SearchTime
        {
            get { return searchTime; }
            set
            {
                searchTime = value;
                NotifyPropertyChanged(m => m.searchTime);
            }
        }
        public string SelectText
        {
            get { return selectText; }
            set
            {
                selectText = value;
                NotifyPropertyChanged(m => m.SelectText);
            }
        }

        

        /// <summary>
        /// 是否正在选择发送咨询/短信目标
        /// </summary>
        private bool isSelect;
        /// <summary>
        /// 是否正在编辑圆圈（overviewOverlay）
        /// </summary>

        private bool isEdit;

        /// <summary>
        /// 查询选中的、需要发送消息的手机号集合
        /// </summary>
        public List<string> PhoneNumbers { get; set; }

        private bool isSendMsgsVisible;
        public bool IsSendMsgsVisible
        {
            get { return isSendMsgsVisible; }
            set
            {
                isSendMsgsVisible = value;
                NotifyPropertyChanged(m => m.IsSendMsgsVisible);
            }
        }

        private bool isSendEmergencyMsgsVisible;
        public bool IsSendEmergencyMsgsVisible
        {
            get { return isSendEmergencyMsgsVisible; }
            set
            {
                isSendEmergencyMsgsVisible = value;
                NotifyPropertyChanged(m => m.IsSendEmergencyMsgsVisible);
            }
        }

        public string content;
        /// <summary>
        /// 咨询内容
        /// </summary>
        public string Content
        {
            get { return content; }
            set
            {
                content = value;
                NotifyPropertyChanged(m => m.Content);
            }
        }

        public string sendText;
        /// <summary>
        /// 短信内容
        /// </summary>
        public string SendText
        {
            get { return sendText; }
            set
            {
                sendText = value;
                NotifyPropertyChanged(m => m.SendText);
            }
        }

        private bool searchBtIsEnabled;
        public bool SearchBtIsEnabled
        {
            get { return searchBtIsEnabled; }
            set
            {
                searchBtIsEnabled = value;
                NotifyPropertyChanged(m => m.SearchBtIsEnabled);
            }
        }

        private int selectedIndex;
        public int SelectedIndex
        {
            get { return selectedIndex; }
            set
            {
                selectedIndex = value;
                NotifyPropertyChanged(m => m.SelectedIndex);
            }
        }

        private Graphic drawGraphics;

        public Graphic DrawGraphics
        {
            get { return drawGraphics; }
            set
            {
                drawGraphics = value;
                NotifyPropertyChanged(m => m.DrawGraphics);
            }
        }

        public ICommand SearchCommand
        {
            get
            {
                return new DelegateCommand(SearchByTime);
            }
        }

        private async void SearchByTime()
        {
            //todo 查询结果，清空，画画
            TouristsList = await GetTouristsLocationsBySomeTime(searchTime_end.AddHours(-1), searchTime_end);
            DrawAgain(true);
        }

        /// <summary>
        /// 清空，再次绘制，区分选中与未选中的
        /// 1、绘制矩形时调用
        /// 2、根据时间检索之后，也会再次绘制（清除矩形）
        /// </summary>
        private void DrawAgain(bool clearRectangle = false)
        {
            _graphicsLayer.Graphics.Clear(); //清空
            for (var n = 0; n < TouristsList.Count; ++n)
            {
                var tourist = TouristsList[n];
                PictureMarkerSymbol symbol; //重新绘制
                if (PhoneNumbers != null && PhoneNumbers.Count != 0 && PhoneNumbers.Contains(tourist.User.PhoneNum))   //选中的项
                {
                    symbol =(touristAgent as IRenderGraphic).CreatSymbol<PictureMarkerSymbol>(new Uri(Global.LocalSelectMarkerUrl));
                }
                else
                {
                    symbol = (touristAgent as IRenderGraphic).CreatSymbol<PictureMarkerSymbol>(new Uri(Global.LocalMarkerUrl));
                }
                var graphic = (touristAgent as IRenderGraphic).CreateGraphic(n, symbol, tourist);
                _graphicsLayer.Graphics.Add(graphic);
            }
            if (clearRectangle)
            {
                ClearRectangle();
            }
        }

        private void ClearRectangle()
        {
            var graphicsOverlay = MyMapView.GraphicsOverlays["overviewOverlay"];
            graphicsOverlay.Graphics.Clear();
        }

        /// <summary>
        /// 获取景区所有用户某段时间内的、最新的位置
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public async Task<List<Tourist>> GetTouristsLocationsBySomeTime(DateTime startTime, DateTime endTime)
        {
            JObject jObject = new JObject();
            jObject.Add("startTime", startTime.ToString());
            jObject.Add("endTime", endTime.ToString());
            string r = await InternetHepler.Instance.PostJObject(jObject, "GetTouristsLocationsBySomeTime");
            List<Tourist> tourists = JsonConvert.DeserializeObject<List<Tourist>>(r);
            return tourists;
        }

        public ICommand CanSearchCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    try
                    {
                        searchTime_end = DateTime.Parse(SearchTime);
                        SearchBtIsEnabled = true;
                    }
                    catch (Exception)
                    {
                        SearchBtIsEnabled = false;
                    }
                });
            }
        }
        

        /// <summary>
        /// 绘制的矩形
        /// </summary>
        private Esri.ArcGISRuntime.Geometry.Geometry geometry;

        public async void MyMapView_MouseDown(object sender, MouseEventArgs e)
        {
            if (isSelect && isEdit==false)    //选择用户状态
            {
                isEdit = true;
                while (isEdit)
                {
                    await DrawRectangle();
                    isEdit = false;
                    Query();
                }
            }
        }

        /// <summary>
        /// 绘制矩形（overviewOverlay）
        /// </summary>
        /// <returns></returns>
        private async Task DrawRectangle()
        {
            var graphicsOverlay = MyMapView.GraphicsOverlays["overviewOverlay"];
            Symbol symbol = new SimpleFillSymbol()
            {
                Color = Color.FromArgb(1,255, 255, 255),
                Outline = new SimpleLineSymbol() {Color = Color.FromRgb(255, 0, 255), Width = 2}
            };
            //画圆找不到半径
            geometry = await MyMapView.Editor.RequestShapeAsync(DrawShape.Rectangle, symbol);
            graphicsOverlay.Graphics.Clear();
            var graphic = new Graphic(geometry, symbol);
            graphicsOverlay.Graphics.Add(graphic);
        }

        /// <summary>
        /// 查询矩形内的用户
        /// </summary>
        void Query()
        {
            var normalizedGeometry = GeometryEngine.NormalizeCentralMeridian(geometry);   //解决经线环绕
            var normalizedGeometry84 = GeometryEngine.Project(normalizedGeometry, SpatialReferences.Wgs84);
            var xmax=normalizedGeometry84.Extent.XMax;
            var xmin = normalizedGeometry84.Extent.XMin;
            var ymax = normalizedGeometry84.Extent.YMax;
            var ymin = normalizedGeometry84.Extent.YMin;

            PhoneNumbers = new List<string>();
            
            for (var n = 0; n < TouristsList.Count; ++n)
            {
                var tourist = TouristsList[n];
                if ((double)tourist.Point.latitude>=ymin 
                    && (double)tourist.Point.latitude<=ymax 
                    && (double)tourist.Point.longitude>=xmin 
                    && (double)tourist.Point.latitude<=xmax)
                {
                    PhoneNumbers.Add(tourist.user.PhoneNum);
                }
            }
            DrawAgain();
        }

        

        /// <summary>
        /// 绘制范围
        /// </summary>
        public ICommand SelectCommond
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    if (isSelect)
                    {
                        SelectText = "选择范围";
                        isSelect = !isSelect;
                    }
                    else
                    {
                        SelectText = "选择中...";
                        isSelect = !isSelect;
                        IsSendEmergencyMsgsVisible = false;
                        IsSendMsgsVisible = false;
                    }
                });
            }
        }

        /// <summary>
        /// 展现发送咨询窗口
        /// </summary>
        public ICommand ShowSendMsgsCommond
        {
            get
            {
                return new RelayCommand(ShowSendMsgs, CanShow);
            }
        }

        void ShowSendMsgs(object obj)
        {
            IsSendMsgsVisible = !IsSendMsgsVisible;
            IsSendEmergencyMsgsVisible = false;
        }

        bool CanShow(object obj)
        {
            if (PhoneNumbers == null || PhoneNumbers.Count == 0)
                return false;
            return true;
        }

        /// <summary>
        /// 发送咨询
        /// </summary>
        public ICommand SendMsgsCommond
        {
            get
            {
                return new DelegateCommand(SendMsgs);
            }
        }

        public void SendMsgs()
        {
            bool r=SendMsgHelper.Instance.SendMsgToTargets(Content, PhoneNumbers);
            if (r)
            {
                MessageBox.Show("发送咨询成功", "操作成功", MessageBoxButton.OK, MessageBoxImage.Information);
                Global.LogServer.SaveServerLogs("针对目标用户发送咨询:"+Content+",成功");
            }
            else
            {
                MessageBox.Show("发送咨询失败，请稍后重试！", "操作失败", MessageBoxButton.OK, MessageBoxImage.Information);
                Global.LogServer.SaveServerLogs("针对目标用户发送咨询:" + Content + ",失败");
            }
        }

        /// <summary>
        /// 展现短信窗口
        /// </summary>
        public ICommand ShowSendEmergencyMsgsCommond
        {
            get
            {
                return new RelayCommand(ShowSendEmergencyMsgs, CanShow);
            }
        }

        void ShowSendEmergencyMsgs(object obj)
        {
            IsSendEmergencyMsgsVisible = !IsSendEmergencyMsgsVisible;
            IsSendMsgsVisible = false;
        }

        /// <summary>
        /// 发送紧急短信
        /// </summary>
        public ICommand SendEmergencyMsgsCommond
        {
            get
            {
                return new DelegateCommand(SendEmergencyMsgs);
            }
        }

        void SendEmergencyMsgs()
        {
            switch (SelectedIndex)
            {
                case 0:
                    RegexTemplate1(@"(?<place>.+?)发生了(?<disaster>.+?)，请尽快撤离，远离该地区。");
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// ()发生了()，请尽快撤离，远离该地区。
        /// </summary>
        /// <param name="regexTemplate"></param>
        private void RegexTemplate1(string regexTemplate)
        {
            Regex regex = new Regex(regexTemplate, RegexOptions.IgnoreCase);
            var result = regex.Match(SendText);
            if (result.Success)
            {
                string place = result.Groups["place"].Value.Trim(new[] { '(', ')' });
                string disaster = result.Groups["disaster"].Value.Trim(new[] { '(', ')' });
                bool r = SendMsgHelper.Instance.SendEmergencyMsgs("1", new List<string>() { place, disaster });
                if (r)
                {
                    MessageBox.Show("发送短信成功！", "操作成功", MessageBoxButton.OK, MessageBoxImage.Information);
                    Global.LogServer.SaveServerLogs("发送短信："+ SendText+"成功");
                }
                else
                {
                    MessageBox.Show("发送请求失败！", "操作失败", MessageBoxButton.OK, MessageBoxImage.Error);
                    Global.LogServer.SaveServerLogs("发送短信：" + SendText + "失败");
                }
            }
            else
            {
                MessageBox.Show("提交内容不符合规范，请重新查看！", "操作失败", MessageBoxButton.OK, MessageBoxImage.Error);
                Global.LogServer.SaveServerLogs("发送短信：" + SendText + "失败，短信不符合规范");
                SendText = @"()发生了()，请尽快撤离，远离该地区。";
            }
        }

        public ICommand SetSendTextCommand
        {
            get { return new DelegateCommand(SetSendText); }
        }

        void SetSendText()
        {
            switch (SelectedIndex)
            {
                case 0:
                    SendText = @"()发生了()，请尽快撤离，远离该地区。";
                    break;
                default:
                    SendText = @"模板错误！";
                    break;
            }
        }

        #endregion

        #region  Commands

        public ICommand LoadTouristInforCommand
        {
            get
            {
                return new DelegateCommand<MapView>(mapView =>
                {
                    MyMapView = mapView;
                    MyMapView.Map.SpatialReference = SpatialReferences.Wgs84;
                    _graphicsLayer = MyMapView.Map.Layers["GraphicsLayer"] as GraphicsLayer;
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

        public ICommand LoadOverViewCommand
        {
            get
            {
                return new DelegateCommand<MapView>(overViewMap =>
                {
                    this.overViewMap = overViewMap;
                    LoadOverView();
                }
                    );
            }
        }

        public ICommand MyMapExtentChangedCommand
        {
            get
            {
                return new DelegateCommand((async () =>
                {
                    if (overViewMap != null && MyMapView != null)
                    {
                        await overViewMap.LayersLoadedAsync();
                        overviewOverlay = overViewMap.GraphicsOverlays["overviewOverlay"];
                        var g = overviewOverlay.Graphics.FirstOrDefault();
                        if (g == null) //first time
                        {
                            g = new Graphic();
                            overviewOverlay.Graphics.Add(g);
                        }
                        var currentViewpoint = MyMapView.GetCurrentViewpoint(ViewpointType.BoundingGeometry);
                        var viewpointExtent = currentViewpoint.TargetGeometry.Extent;
                        g.Geometry = viewpointExtent;

                        await overViewMap.SetViewAsync(viewpointExtent.GetCenter(), MyMapView.Scale*15);
                    }
                }));
            }
        }

        #endregion
    }
}