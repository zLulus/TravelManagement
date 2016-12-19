using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using FirstFloor.ModernUI.Presentation;
using SimpleMvvmToolkit;
using TravelManagement.Models;
using TravelManagement.Views;
using TravelManagement.Views.Contents;
using ModernUINavigation.Pages;
using TravelManagement.Views.Pages;

namespace TravelManagement.ViewModels
{
    public class ModelsUserControlViewModel:ViewModelBase<ModelsUserControlViewModel>
    {
        private ObservableCollection<Model> models;
        private Model selectedModel;

        public Model SelectedModel
        {
            get { return selectedModel; }
            set
            {
                selectedModel = value;
                NotifyPropertyChanged(m => m.SelectedModel);
            }
        }

        public ObservableCollection<Model> Models
        {
            get { return models; }
            set
            {
                models = value;
                NotifyPropertyChanged(m => m.Models);
            }
        }

        public ModelsUserControlViewModel()
        {
            Models = new ObservableCollection<Model>();
        }

        //private void UIElement_OnMouseWheel(object sender, MouseWheelEventArgs e)
        //{
        //    ScrollViewer scroll = sender as ScrollViewer;
        //    ListBox box=new ListBox();
            
        //    scroll.ScrollToVerticalOffset(scroll.VerticalOffset - e.Delta * 0.5);
        //}

        /// <summary>
        /// 初始化模块
        /// </summary>
        public ICommand LoadCommand
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    if (Models.Count == 0)
                    {
                        Models.Add(new Model() { Title = "景区基础底图设置", ModelImage = GetIco("景区基础底图设置") });
                        Models.Add(new Model() { Title = "景区监控", ModelImage = GetIco("景区监控") });
                        Models.Add(new Model() { Title = "异常情况", ModelImage = GetIco("异常情况") });
                        Models.Add(new Model() { Title = "紧急呼救", ModelImage = GetIco("紧急呼救") });
                        Models.Add(new Model() { Title = "景区紧急消息推送", ModelImage = GetIco("景区紧急消息推送") });
                        Models.Add(new Model() { Title = "景区咨询推送", ModelImage = GetIco("景区咨询推送") });
                        Models.Add(new Model() { Title = "系统设置", ModelImage = GetIco("系统设置") });
                    }
                });
            }
        }

        /// <summary>
        /// 获取模块图标
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private static ImageSource GetIco(string code)
        {
            string fileName;
            switch (code)
            {
                case "景区基础底图设置":
                    fileName = @"..\..\Resources\Image\0106.png";
                    break;
                case "景区监控":
                    fileName = @"..\..\Resources\Image\0105.png";
                    break;
                case "异常情况":
                    fileName = @"..\..\Resources\Image\0318.png";
                    break;
                case "紧急呼救":
                    fileName = @"..\..\Resources\Image\phone.png";
                    break;
                case "景区紧急消息推送":
                    fileName = @"..\..\Resources\Image\03.png";
                    break;
                case "景区咨询推送":
                    fileName = @"..\..\Resources\Image\0603.png";
                    break;
                case "系统设置":
                    fileName = @"..\..\Resources\Image\01.png";
                    break;
                default:
                    return null;
            }
            Stream reader = File.OpenRead(fileName); ;
            System.Drawing.Image photo = System.Drawing.Image.FromStream((Stream)reader);
            MemoryStream finalStream = new MemoryStream();
            photo.Save(finalStream, System.Drawing.Imaging.ImageFormat.Png);
            PngBitmapDecoder decoder = new PngBitmapDecoder(finalStream, BitmapCreateOptions.PreservePixelFormat,
                BitmapCacheOption.Default);
            return decoder.Frames[0];
        }

        /// <summary>
        /// 选中项改变（点击不同模块）
        /// </summary>
        public ICommand SelectionChangedCommand
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    if (SelectedModel == null)
                    {
                        return;
                    }
                    switch (SelectedModel.Title)
                    {
                        case "景区基础底图设置":
                            Global.CurrView = new ScenicSpotBasicSetting();
                            break;
                        case "景区监控":
                            Global.CurrView = new ScenicSpotMonitoring();
                            break;
                        case "异常情况":
                            Global.CurrView = new LostContactStatistics();
                            break;
                        case "紧急呼救":
                            Global.CurrView = new EmergencyCallStatistics();
                            break;
                        case "景区紧急消息推送":
                            Global.CurrView =new SendEmergencyMsgs();
                            break;
                        case "景区咨询推送":
                            Global.CurrView = new SendMsgs();
                            break;
                        case "系统设置":
                            Global.CurrView=new SettingsPage();
                            break;
                        default:
                            return;
                    }
                    SelectedModel = null;
                    if (Global.CurrView != null)
                    {
                        Global.MainWindow.ViewContiner.Children.Clear();
                        Global.MainWindow.ViewContiner.Children.Add(Global.CurrView);
                        Global.ViewStack.Push(Global.CurrView);
                        (Global.MainWindow.DataContext as MainWindowViewModel).GoBackBtChange();
                    }
                });
            }
        }
    }
}
