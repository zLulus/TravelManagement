using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using SimpleMvvmToolkit;
using TravelManagement.Services;
using TravelManagement.Models;

namespace TravelManagement.ViewModels
{
    public class PedestrianVolumeViewModel : ViewModelDetailBase<PedestrianVolumeViewModel, PedestrianVolume>
    {

        private PedestrianVolumeServer serverAgent;
        
        public PedestrianVolumeViewModel(PedestrianVolumeServer server)
        {
            serverAgent = server;
            base.Model = serverAgent.ReturnPedestrianVolume();
        }



        #region Methods

        public void Statistics()
        {
            serverAgent.Statistics();
        }

        #endregion

        #region Commands

        public ICommand StatisticsCommand
        {
            get
            {
                return new DelegateCommand(Statistics);
            }
        }

        #endregion

    }
}
