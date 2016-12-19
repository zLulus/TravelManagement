using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using FirstFloor.ModernUI.Presentation;
using SimpleMvvmToolkit;
using TravelManagement.Models;
using TravelManagement.Services;

namespace TravelManagement.ViewModels
{
    public class AddAdminViewModel: ViewModelBase<AddAdminViewModel>
    {
        private AddAdminServer serverAgent;

        public AddAdminViewModel(AddAdminServer server)
        {
            serverAgent = server;
            Admin=new Administrator();
        }

        #region Properties
        public string repeatPassword;
        
        public string RepeatPassword
        {
            get { return repeatPassword; }
            set
            {
                repeatPassword = value;
                NotifyPropertyChanged(m => m.RepeatPassword);
            }
        }

        public Administrator admin;

        public Administrator Admin
        {
            get { return admin; }
            set
            {
                admin = value;
                NotifyPropertyChanged(m => m.Admin);
            }
        }

        private PasswordBox passwordBox1;
        private PasswordBox passwordBox2;
        #endregion

        public void AddAdmin(object obj)
        {
            if (Admin.Password!=RepeatPassword)
            {
                MessageBox.Show("两次输入密码不一致！", "错误信息", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            Admin.CreatTime = DateTime.Now;
            if (serverAgent.AddAdmin(Admin))
            {
                MessageBox.Show("添加管理员账号成功！", "操作成功", MessageBoxButton.OK, MessageBoxImage.Information);
                Admin=new Administrator();
                passwordBox1.Password = null;
                passwordBox2.Password = null;
            }
            else
            {
                MessageBox.Show("添加管理员账号失败！", "错误信息", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public bool CanExecuteAddCommand(object obj)
        {
            if (string.IsNullOrEmpty(Admin.Password) || string.IsNullOrEmpty(RepeatPassword)
                || string.IsNullOrEmpty(Admin.Account) || string.IsNullOrEmpty(Admin.Name))
                return false;
            return true;
        }

        public ICommand AddCommand
        {
            get
            {
                return new RelayCommand(AddAdmin, CanExecuteAddCommand);
            }
        }

        public ICommand PasswordChangedCommand1
        {
            get
            {
                return new DelegateCommand<PasswordBox>((passwordBox) =>
                {
                    Admin.Password = passwordBox.Password;
                    passwordBox1 = passwordBox;
                });
            }
        }

        public ICommand PasswordChangedCommand2
        {
            get
            {
                return new DelegateCommand<PasswordBox>((passwordBox) =>
                {
                    RepeatPassword = passwordBox.Password;
                    passwordBox2 = passwordBox;
                });
            }
        }
    }
}
