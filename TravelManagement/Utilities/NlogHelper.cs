using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace TravelManagement.Utilities
{
    public class NlogHelper
    {
        private static LoggingConfiguration Config = null;

        #region  Instance
        private static NlogHelper _instance = null;
        private static readonly object SyncRoot = new object();

        public static NlogHelper Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (SyncRoot)
                    {
                        if (null == _instance)
                        {
                            _instance = new NlogHelper();
                        }
                    }
                }
                return _instance;
            }
        }
        #endregion
        private void Configuration(string loggerName)
        {
            Config = new LoggingConfiguration();
            var fileTarget = new FileTarget()  //文件log
            {
                FileName = @"${basedir}/output/" + loggerName + ".log",    //存储位置
                Layout = @"${date:format=yyyy-MM-dd HH\:mm\:ss} ${message}",  //格式
                Encoding = Encoding.Default
            };
            Config.LoggingRules.Add(new LoggingRule(loggerName, LogLevel.Debug, fileTarget));  //添加记录规则
            LogManager.Configuration = Config;   //将配置赋值给LogManager
        }
        public Logger GetFileLogger(string loggerName)
        {
            if (Config == null)
                Configuration(loggerName);
            return LogManager.GetLogger(loggerName);
        }

        public void SaveLog(string msg)
        {
            string logName = DateTime.Today.ToLongDateString();   //取名：2016年7月21日.log
            Logger _logger = GetFileLogger(logName);
            //${message}部分
            _logger.Info(msg);
        }
    }
}
