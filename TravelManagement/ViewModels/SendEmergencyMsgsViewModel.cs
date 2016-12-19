using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using SimpleMvvmToolkit;
using TravelManagement.Services;

namespace TravelManagement.ViewModels
{
    public class SendEmergencyMsgsViewModel:ViewModelBase<SendEmergencyMsgsViewModel>
    {
        private SendEmergencyMsgsServer serverAgent;

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

        private string sendText;
        public string SendText
        {
            get { return sendText; }
            set
            {
                sendText = value;
                NotifyPropertyChanged(m => m.SendText);
            }
        }

        private string exampleText;
        public string ExampleText
        {
            get { return exampleText; }
            set
            {
                exampleText = value;
                NotifyPropertyChanged(m => m.ExampleText);
            }
        }

        private string tipText;
        public string TipText
        {
            get { return tipText; }
            set
            {
                tipText = value;
                NotifyPropertyChanged(m => m.TipText);
            }
        }


        public SendEmergencyMsgsViewModel(SendEmergencyMsgsServer server)
        {
            serverAgent = server;
            SelectedIndex = 0;
            TipText = @"请在()内填写内容，括号外填写的所有内容无效！";
            SetSendText();
        }

        public ICommand SetSendTextCommand
        {
            get { return new DelegateCommand(SetSendText);}
        }

        void SetSendText()
        {
            switch (SelectedIndex)
            {
                case 0:
                    SendText = @"()发生了()，请尽快撤离，远离该地区。";
                    ExampleText = @"(峨眉山东部)发生了(泥石流)，请尽快撤离，远离该地区。";
                    break;
                default:
                    SendText = @"模板错误！";
                    break;
            }
        }

        public ICommand SendCommand
        {
            get
            {
                return new DelegateCommand(SendEmergencyMsgs);
            }
        }

        /// <summary>
        /// 总函数，进行模板分类
        /// </summary>
        private void SendEmergencyMsgs()
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
            var result= regex.Match(SendText);
            if (result.Success)
            {
                string place = result.Groups["place"].Value.Trim(new []{'(',')'});
                string disaster = result.Groups["disaster"].Value.Trim(new[] { '(', ')' });
                bool r=serverAgent.SendEmergencyMsgs("1", new List<string>() {place, disaster});
                if(r)
                    MessageBox.Show("发送短信成功！", "操作成功", MessageBoxButton.OK, MessageBoxImage.Information);
                else
                {
                    MessageBox.Show("发送请求失败！", "操作失败", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("提交内容不符合规范，请重新查看！", "操作失败", MessageBoxButton.OK, MessageBoxImage.Error);
                SendText = @"()发生了()，请尽快撤离，远离该地区。";
            }
        }
    }
}
