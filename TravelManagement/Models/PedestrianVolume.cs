using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using SimpleMvvmToolkit;

namespace TravelManagement.Models
{
    public class PedestrianVolume: ModelBase<PedestrianVolume>
    {
        private ObservableCollection<PedestrianVolumeModel> pedestrianVolumes;

        public ObservableCollection<PedestrianVolumeModel> PedestrianVolumes
        {
            get { return pedestrianVolumes; }
            set
            {
                pedestrianVolumes = value;
                NotifyPropertyChanged(m => m.PedestrianVolumes);
            }

        }

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

        private ComboBoxItem selectedItem;
        public ComboBoxItem SelectedItem
        {
            get { return selectedItem; }
            set
            {
                selectedItem = value;
                NotifyPropertyChanged(m => m.SelectedItem);
            }
        }

        public PedestrianVolume()
        {
            PedestrianVolumes = new ObservableCollection<PedestrianVolumeModel>();
            StartTime = DateTime.Now;
        }
    }

    public class PedestrianVolumeModel : ModelBase<PedestrianVolumeModel>
    {
        public DateTime startTime;

        /// <summary>
        /// 起始时间
        /// </summary>
        public DateTime StartTime
        {
            get { return startTime; }
            set
            {
                startTime = value;
                NotifyPropertyChanged(m => m.StartTime);
            }
        }

        public DateTime endTime;

        /// <summary>
        /// 终止时间
        /// </summary>
        public DateTime EndTime
        {
            get { return endTime; }
            set
            {
                endTime = value;
                NotifyPropertyChanged(m => m.EndTime);
            }
        }

        private int number;

        /// <summary>
        /// 人流量
        /// </summary>
        public int Number
        {
            get { return number; }
            set
            {
                number = value;
                NotifyPropertyChanged(m => m.Number);
            }
        }
    }
}
