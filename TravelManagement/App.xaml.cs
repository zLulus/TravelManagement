using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using TravelManagement.Utilities;

namespace TravelManagement
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

         public App()
        {
            //处理UI线程异常
            Application.Current.DispatcherUnhandledException += CurrentDomain_UnhandledException;
        }


        private static void CurrentDomain_UnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            var str = "";
            var error = e.Exception;
            var strDateInfo = "出现应用程序未处理的异常：" + DateTime.Now + "\r\n";
            if (error != null)
            {
                str = string.Format(strDateInfo + "Application UnhandledException:{0};\n\r堆栈信息:{1}", error.Message,
                    error.StackTrace);
            }
            else
            {
                str = string.Format("Application UnhandledError:{0}", e);
            }
            MessageBox.Show("很抱歉，当前程序遇到一些问题，该操作已终止，请检查网络连接，如果问题依然存在，请联系管理员", "意外的操作", MessageBoxButton.OK,
                MessageBoxImage.Information);
            NlogHelper.Instance.SaveLog("TargetSite:" + error.TargetSite + ",Source:" + error.Source + ",Message:" + error.Message);
            e.Handled = true;
        }



    }
}
