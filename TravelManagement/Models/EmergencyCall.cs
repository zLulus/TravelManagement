using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using SimpleMvvmToolkit;

namespace TravelManagement.Models
{
    /// <summary>
    /// 总model
    /// </summary>
    public class EmergencyCall : ModelBase<EmergencyCall>
    {
        private ObservableCollection<EmergencyCallModel> emergencyCallCollection;

        public ObservableCollection<EmergencyCallModel> EmergencyCallCollection
        {
            get { return emergencyCallCollection; }
            set
            {
                emergencyCallCollection = value;
                NotifyPropertyChanged(m => m.EmergencyCallCollection);
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

        public EmergencyCall()
        {
            emergencyCallCollection=new ObservableCollection<EmergencyCallModel>();
            EndTime = DateTime.Now;
            StartTime = DateTime.Now.AddDays(-7);
        }
    }

    /// <summary>
    /// 单项紧急呼救Model
    /// </summary>
    public class EmergencyCallModel : ModelBase<EmergencyCallModel>
    {
        [DataMember]
        private int id;
        [IgnoreDataMember]
        public int ID
        {
            get { return id; }
            set
            {
                id = value;
                NotifyPropertyChanged(m => m.ID);
            }
        }
        [DataMember]
        private string phoneNum;
        [IgnoreDataMember]
        public string PhoneNum
        {
            get { return phoneNum; }
            set
            {
                phoneNum = value;
                NotifyPropertyChanged(m => m.PhoneNum);
            }
        }
        [DataMember]
        private string time;
        [IgnoreDataMember]
        public string Time
        {
            get { return time; }
            set
            {
                time = value;
                NotifyPropertyChanged(m => m.Time);
            }
        }
        [DataMember]
        private decimal? longitude;

        /// <summary>
        /// 经度
        /// </summary>
        [IgnoreDataMember]
        public decimal? Longitude
        {
            get { return longitude; }
            set
            {
                longitude = value;
                NotifyPropertyChanged(m => m.Longitude);
            }
        }
        [DataMember]
        private decimal? latitude;
        /// <summary>
        /// 纬度
        /// </summary>
        [IgnoreDataMember]
        public decimal? Latitude
        {
            get { return latitude; }
            set
            {
                latitude = value;
                NotifyPropertyChanged(m => m.Latitude);
            }
        }
    }
}
