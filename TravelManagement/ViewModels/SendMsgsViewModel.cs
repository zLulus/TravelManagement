using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using FirstFloor.ModernUI.Presentation;
using SimpleMvvmToolkit;
using TravelManagement.Services;

namespace TravelManagement.ViewModels
{
    public class SendMsgsViewModel:ViewModelBase<SendMsgsViewModel>
    {
        private SendMsgsServer serverAgent;

        public string content;

        public string Content
        {
            get { return content; }
            set
            {
                content = value;
                NotifyPropertyChanged(m => m.Content);
            }
        }

        public SendMsgsViewModel(SendMsgsServer server)
        {
            serverAgent = server;
        }

        public ICommand SendCommand
        {
            get
            {
                return new RelayCommand(SendMsg, CanExecuteSendMsgCommand);
            }
        }

        public void SendMsg(object obj)
        {
            if (string.IsNullOrEmpty(Content))
            {
                MessageBox.Show("请输入广播内容", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            bool r =serverAgent.SendMsg(content);
            if (r)
            {
                MessageBox.Show("推送消息成功", "操作成功", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("推送消息失败", "操作失败", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        public bool CanExecuteSendMsgCommand(object obj)
        {
            //todo 输入即可用
            //if (string.IsNullOrEmpty(Content))
            //    return false;
            return true;
        }
    }
}
