using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using SimpleMvvmToolkit;
using TravelManagement.Models;
using TravelManagement.Services;

namespace TravelManagement.ViewModels
{
    public class LogInViewModel:ViewModelBase<LogInViewModel>
    {
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

        private LogInServer serverAgent;

        public LogInViewModel(LogInServer server)
        {
            serverAgent = server;
            Admin=new Administrator();
        }

        public async void LogIn()
        {
            if (string.IsNullOrEmpty(admin.Account) || string.IsNullOrEmpty(admin.Password))
            {
                MessageBox.Show("请输入账号、密码！", "提示");
                return;
            }
            bool result = await serverAgent.LogIn(admin.Account, admin.Password);
            if (result)
            {
                MainWindow window = new MainWindow();
                window.Show();
                Application.Current.MainWindow.Close();
            }
            else
            {
                MessageBox.Show("操作失败！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public ICommand LogInCommand => new DelegateCommand(LogIn);

        public ICommand PasswordChangedCommand
        {
            get
            {
                //todo 判断回车即登录
                return new DelegateCommand<PasswordBox>((passwordBox) =>
                {
                    Admin.Password = passwordBox.Password;
                });
            }
        }
    }
}
