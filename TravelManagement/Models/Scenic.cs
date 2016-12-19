using System.Runtime.Serialization;
using SimpleMvvmToolkit;

namespace TravelManagement.Models
{
  
    public class Scenic : ModelBase<Scenic>
    {
       [DataMember]
        public string description;

        [IgnoreDataMember]
        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                NotifyPropertyChanged(m => m.Description);
            }
        }

         [DataMember]
        public int id;

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
        public string name;

        [IgnoreDataMember]
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                NotifyPropertyChanged(m => m.Name);
            }
        }

         [DataMember]
        public decimal? longitude;

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
        public decimal? latitude;

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

