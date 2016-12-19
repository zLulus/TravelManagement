using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleMvvmToolkit;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace TravelManagement.Models
{
    public class LostContact : ModelBase<LostContact>
    {
        private ObservableCollection<LostContactModel> lostContactCollection;
        private LostContactModel selected;

        public LostContactModel Selected
        {
            get { return selected; }
            set
            {
                selected = value;
                NotifyPropertyChanged(m => m.Selected);
            }

        }

        public ObservableCollection<LostContactModel> LostContactCollection
        {
            get { return lostContactCollection; }
            set
            {
                lostContactCollection = value;
                NotifyPropertyChanged(m => m.LostContactCollection);
            }

        }

        private bool hasSolved;
        /// <summary>
        /// 勾选项是否包含已解决
        /// </summary>
        public bool HasSolved
        {
            get { return hasSolved; }
            set
            {
                hasSolved = value;
                NotifyPropertyChanged(m => m.HasSolved);
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

        public LostContact()
        {
            lostContactCollection = new ObservableCollection<LostContactModel>();
            EndTime = DateTime.Now;
            StartTime = DateTime.Now.AddDays(-7);
            HasSolved = false;
        }
    }
    public class LostContactModel : ModelBase<LostContactModel>
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
        private User user;
        [IgnoreDataMember]
        public User MyUser
        {
            get { return user; }
            set
            {
                user = value;
                NotifyPropertyChanged(m => m.MyUser);
            }
        }

        [DataMember]
        private string searchMoment;
        [IgnoreDataMember]
        public string SearchMoment
        {
            get { return searchMoment; }
            set
            {
                searchMoment = value;
                NotifyPropertyChanged(m => m.SearchMoment);
            }
        }

        [DataMember]
        public string state;

        /// <summary>
        /// 异常核查进展
        /// </summary>
        [IgnoreDataMember]
        public string State
        {
            get { return state; }
            set
            {
                state = value;
                NotifyPropertyChanged(m => m.State);
            }
        }

        /// <summary>
        /// 异常级别
        /// 0-异常 1-危急
        /// </summary>
        [DataMember]
        private int? grade;
        [IgnoreDataMember]
        public int? Grade
        {
            get { return grade; }
            set
            {
                grade = value;
                NotifyPropertyChanged(m => m.Grade);
            }
        }

        [DataMember]
        private int? isSolved;

        /// <summary>
        /// 是否已解决异常
        /// 0-未解决 1-已解决
        /// </summary>
        [IgnoreDataMember]
        public int? IsSolved
        {
            get { return isSolved; }
            set
            {
                isSolved = value;
                NotifyPropertyChanged(m => m.IsSolved);
            }
        }
    }
}
