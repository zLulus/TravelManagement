using System.Collections.Generic;
using System.Runtime.Serialization;
using SimpleMvvmToolkit;

namespace TravelManagement.Models
{
    /// <summary>
    /// 某游客当前位置
    /// </summary>
    public class Tourist:ViewModelBase<Tourist>
    {
        public User user;

        [IgnoreDataMember]
        public User User
        {
            get { return user; }
            set
            {
                user = value;
                NotifyPropertyChanged(m => m.User);
            }
        }



        public Point point;

        [IgnoreDataMember]
        public Point Point
        {
            get { return point; }
            set
            {
                point = value;
                NotifyPropertyChanged(m => m.Point);
            }
        }

     

    }
}
