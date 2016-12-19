using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using SimpleMvvmToolkit;
using TravelManagement.Models;
using TravelManagement.Services;

namespace TravelManagement.ViewModels
{
    public class LostContactViewModel : ViewModelDetailBase<LostContactViewModel, LostContact>
    {
        private LostContactServer agentServer;
        public LostContactViewModel(LostContactServer server)
        {
            agentServer = server;
            Model = agentServer.GetLostContact();
        }

        public void Statistics()
        {
            agentServer.Statistics();
        }

        public ICommand StatisticsCommand
        {
            get { return new DelegateCommand(Statistics);}
        }

        public ICommand ResolveCommand
        {
            get { return new DelegateCommand(RecordSolved); }
        }

        public void RecordSolved()
        {
            if (Model.Selected.IsSolved == 1)
            {
                MessageBox.Show("该异常情况已处理，不能进行操作！", "提示");
                return;
            }
            Task.Run(async () =>
            {
                bool r=await agentServer.RecordSolved();
                if (r)
                {
                    Model.Selected.IsSolved = 1;
                    MessageBox.Show("记录异常情况已处理，操作成功", "提示");
                }
                else
                {
                    Model.Selected.IsSolved = 0;
                    MessageBox.Show("记录异常情况已处理，操作失败，请稍后再试！", "提示");
                }
            });
            
        }

    }
}
