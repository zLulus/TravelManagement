using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using SimpleMvvmToolkit;

namespace TravelManagement.Models
{
    public class Administrator : ViewModelBase<Administrator>
    {
        public string account;
        [IgnoreDataMember]
        public string Account
        {
            get { return account; }
            set
            {
                account = value;
                NotifyPropertyChanged(m => m.Account);
            }
        }

        public string password;

        [IgnoreDataMember]
        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                NotifyPropertyChanged(m => m.Password);
            }
        }

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

        public string sex;

        [IgnoreDataMember]
        public string Sex
        {
            get { return sex; }
            set
            {
                sex = value;
                NotifyPropertyChanged(m => m.Sex);
            }
        }

        public string position;

        [IgnoreDataMember]
        public string Position
        {
            get { return position; }
            set
            {
                position = value;
                NotifyPropertyChanged(m => m.Position);
            }
        }

        public Nullable<System.DateTime> creatTime;


        [IgnoreDataMember]
        public Nullable<System.DateTime> CreatTime
        {
            get { return creatTime; }
            set
            {
                creatTime = value;
                NotifyPropertyChanged(m => m.CreatTime);
            }
        }

        //public Nullable<int> isDelete;

        //[IgnoreDataMember]
        //public Nullable<int> IsDelete
        //{
        //    get { return isDelete; }
        //    set
        //    {
        //        isDelete = value;
        //        NotifyPropertyChanged(m => m.IsDelete);
        //    }
        //}

    }
}
