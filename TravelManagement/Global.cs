using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelManagement.Models;
using TravelManagement.Services;
using System.Windows.Controls;

namespace TravelManagement
{
    public class Global
    {
        public static string url = "...";
        //public static string url = "http://localhost:15193/WCFService.svc/service";

        public static Administrator LogAdministrator = null;

        /// <summary>
        /// 用于记录管理员操作
        /// </summary>
        public static ServerLogServer LogServer=new ServerLogServer();

        public static string LocalFeatureUrl=String.Empty;
        public static string LocalMarkerUrl = "pack://application:,,,/SimpleMvvm_Wpf;component/Resources/Image/location_marker.png";
        public static string LocalSelectMarkerUrl = "pack://application:,,,/SimpleMvvm_Wpf;component/Resources/Image/location_marker_select.png";

        public static Stack<UserControl> ViewStack=new Stack<UserControl>();
        public static UserControl CurrView;
        public static MainWindow MainWindow;
    }
}
