using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using SimpleMvvmToolkit;

namespace TravelManagement.Models
{
    public class ServerLog : ModelBase<ServerLog>
    {
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
        public DateTime? time;

        [IgnoreDataMember]
        public DateTime? Time
        {
            get { return time; }
            set
            {
                time = value;
                NotifyPropertyChanged(m => m.Time);
            }
        }

        [DataMember]
        public string content;

        [IgnoreDataMember]
        public string Content
        {
            get { return content; }
            set
            {
                content = value;
                NotifyPropertyChanged(m => m.Content);
            }
        }
    }
}
