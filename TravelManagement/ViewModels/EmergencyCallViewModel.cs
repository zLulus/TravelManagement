using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using SimpleMvvmToolkit;
using TravelManagement.Models;
using TravelManagement.Services;

namespace TravelManagement.ViewModels
{
    public class EmergencyCallViewModel : ViewModelDetailBase<EmergencyCallViewModel, EmergencyCall>
    {
        private EmergencyCallServer agentServer;

        public EmergencyCallViewModel(EmergencyCallServer server)
        {
            agentServer = server;
            Model = agentServer.GetEmergencyCall();
        }

        public void Statistics()
        {
            agentServer.Statistics();
        }

        public ICommand StatisticsCommand
        {
            get
            {
                return new DelegateCommand(Statistics);
            }
        }
    }
}
