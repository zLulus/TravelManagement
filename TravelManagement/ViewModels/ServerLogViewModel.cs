using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TravelManagement.Services;
using SimpleMvvmToolkit;
using TravelManagement.Models;

namespace TravelManagement.ViewModels
{
    public class ServerLogViewModel : ViewModelBase<ServerLogViewModel>
    {
        private ServerLogServer serverAgent;

        private DateTime startTime;
        public DateTime StartTime
        {
            get { return startTime; }
            set
            {
                startTime = value;
                NotifyPropertyChanged(m => m.StartTime);
            }
        }

        private DateTime endTime;
        public DateTime EndTime
        {
            get { return endTime; }
            set
            {
                endTime = value;
                NotifyPropertyChanged(m => m.EndTime);
            }
        }

        private ObservableCollection<ServerLog> serverLogs;

        public ObservableCollection<ServerLog> ServerLogs
        {
            get { return serverLogs; }
            set
            {
                serverLogs = value;
                NotifyPropertyChanged(m => m.ServerLogs);
            }
        }

        public ServerLogViewModel(ServerLogServer server)
        {
            serverAgent = server;
            StartTime = DateTime.Now.AddDays(-7);
            EndTime = DateTime.Now;
            Task.Run(async () =>
            {
                ServerLogs = await serverAgent.GetServerLogs(startTime, endTime);
            });
        }

        public ICommand SearchCommand
        {
            get
            {
                return new DelegateCommand(async () =>
                {
                    ServerLogs=await serverAgent.GetServerLogs(startTime, endTime);
                });
            }
        }
    }
}
