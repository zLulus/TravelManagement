using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using FirstFloor.ModernUI.Presentation;
using SimpleMvvmToolkit;
using SimpleMvvmToolkit.ModelExtensions;
using TravelManagement.Models;
using TravelManagement.Services;
using TravelManagement.Views.Dialog;

namespace TravelManagement.ViewModels
{
    public class CheckAdminViewModel:ViewModelBase<CheckAdminViewModel>
    {
        #region 属性
        private CheckAdminServer serverAgent;

        private ObservableCollection<Administrator> administrators;

        /// <summary>
        /// DataGird显示
        /// </summary>
        public ObservableCollection<Administrator> Administrators
        {
            get { return administrators; }
            set
            {
                administrators = value;
                NotifyPropertyChanged(m => m.Administrators);
            }
        }

        private ObservableCollection<Administrator> totalAdministrators;

        /// <summary>
        /// 所有
        /// </summary>
        public ObservableCollection<Administrator> TotalAdministrators
        {
            get { return totalAdministrators; }
            set
            {
                totalAdministrators = value;
                NotifyPropertyChanged(m => m.TotalAdministrators);
            }
        }

        private Administrator selectAdmin;
        /// <summary>
        /// DataGrid选中项
        /// </summary>
        public Administrator SelectAdmin
        {
            get { return selectAdmin; }
            set
            {
                selectAdmin = value;
                NotifyPropertyChanged(m => m.SelectAdmin);
            }
        }

        private Administrator alterAdmin;
        /// <summary>
        /// 用于修改信息的
        /// </summary>
        public Administrator AlterAdmin
        {
            get { return alterAdmin; }
            set
            {
                alterAdmin = value;
                NotifyPropertyChanged(m => m.AlterAdmin);
            }
        }

        public string repeatPsw;

        public bool isEnabled;
        /// <summary>
        /// 搜索栏是否可用（查询完成前不可用）
        /// </summary>
        public bool IsEnabled
        {
            get { return isEnabled; }
            set
            {
                isEnabled = value;
                NotifyPropertyChanged(m => m.IsEnabled);
            }
        }

        #endregion

        #region basic
        public CheckAdminViewModel(CheckAdminServer server)
        {
            serverAgent = server;
            IsEnabled = false;   //默认不可用
            Task.Run(async () =>
            {
                TotalAdministrators = await serverAgent.GetAdministrators();
                Administrators = TotalAdministrators;
                IsEnabled = true;   //抓取数据完成可用
            }); 
        }

        public ICommand SelectionChangedCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    AlterAdmin = new Administrator();
                    SelectAdmin.CopyValuesTo(AlterAdmin);
                });
            }
        }
        #endregion

        #region 删除
        public ICommand DeleteCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    MessageBoxResult result=MessageBox.Show("确定删除该账号？", "操作确认", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        if (serverAgent.DeleteAdmin(SelectAdmin))
                        {
                            MessageBox.Show("删除账号成功！", "操作成功", MessageBoxButton.OK);
                            Administrators.Remove(SelectAdmin);
                        }
                        else
                        {
                            MessageBox.Show("删除账号失败！", "操作失败", MessageBoxButton.OK,MessageBoxImage.Error);
                        }
                    }
                });
            }
        }

        #endregion

        #region 修改信息

        public ICommand AlterInfoCommand
        {
            get
            {
                return new RelayCommand(AlterInfo, CanExecuteAlterInfoCommand);
            }
        }

        private void AlterInfo(object obj)
        {
            if (string.IsNullOrEmpty(AlterAdmin.Name))
            {
                MessageBox.Show("姓名不能为空！", "输入错误", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            Task.Run(async () =>
            {
                bool r = await serverAgent.AlterInfo(AlterAdmin.Account, AlterAdmin.Name, AlterAdmin.Sex, AlterAdmin.Position);
                if (r)
                {
                    MessageBox.Show("修改信息成功！", "操作成功", MessageBoxButton.OK, MessageBoxImage.Information);
                    AlterAdmin.CopyValuesTo(selectAdmin);
                }
                else
                {
                    MessageBox.Show("修改信息失败！", "操作失败", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            });
        }

        /// <summary>
        /// 控制按钮可用不可用
        /// </summary>
        /// <param name="textBox"></param>
        /// <returns></returns>
        public bool CanExecuteAlterInfoCommand(object obj)
        {
            if (alterAdmin == null || SelectAdmin == null)   //初始情况
                return false;
            if (AlterAdmin.Name == SelectAdmin.Name &&
                AlterAdmin.Sex == SelectAdmin.Sex &&
                AlterAdmin.Position == SelectAdmin.Position)
                return false;
            return true;
        }
        #endregion

        #region 修改密码
        public ICommand PasswordChangedCommand1
        {
            get
            {
                return new DelegateCommand<PasswordBox>((passwordBox) =>
                {
                    AlterAdmin.Password = passwordBox.Password;
                });
            }
        }

        public ICommand PasswordChangedCommand2
        {
            get
            {
                return new DelegateCommand<PasswordBox>((passwordBox) =>
                {
                    repeatPsw = passwordBox.Password;
                });
            }
        }

        public ICommand AlterPswCommand
        {
            get
            {
                return new RelayCommand(AlterPsw, CanExecuteAlterPswCommand);
            }
        }

        private void AlterPsw(object obj)
        {
            if (AlterAdmin.Password != repeatPsw)
            {
                MessageBox.Show("两次输入密码不一致！", "输入错误", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            Task.Run(async () =>
            {
                bool r = await serverAgent.AlterPassword(AlterAdmin.Account, AlterAdmin.Password);
                if (r)
                    MessageBox.Show("修改密码成功！", "操作成功", MessageBoxButton.OK, MessageBoxImage.Information);
                else
                {
                    MessageBox.Show("修改密码失败！", "操作失败", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            });
            
        }

        public bool CanExecuteAlterPswCommand(object obj)
        {
            if (AlterAdmin == null)
                return false;
            if (string.IsNullOrEmpty(AlterAdmin.Account))
                return false;
            if (string.IsNullOrEmpty(AlterAdmin.Password) || string.IsNullOrEmpty(repeatPsw))
                return false;
            return true;
        }

        #endregion

        //public ICommand RefreshCommand
        //{
        //    get
        //    {
        //        return new DelegateCommand<TextBox>(async (textBox) =>
        //        {
        //            if (string.IsNullOrEmpty(textBox.Text))    //显示全部，网上查询
        //            {
        //                Administrators = await serverAgent.GetAdministrators();
        //            }
        //            else     //进行搜索
        //            {
        //                Search(textBox.Text);
        //            }
        //        });
        //    }
        //}

        #region 搜索
        private void Search(string searchText)
        {
            ObservableCollection<Administrator> results = new ObservableCollection<Administrator>();
            foreach (var admin in TotalAdministrators)
            {
                try   //防止职位什么的为空
                {
                    if (admin.Account.Contains(searchText)
                    || admin.name.Contains(searchText)
                    || admin.position.Contains(searchText))    
                        results.Add(admin);
                }
                catch (Exception)
                {
                    
                }
                
            }
            Administrators = results;
        }

        public ICommand TextChangedCommand
        {
            get
            {
                return new DelegateCommand<TextBox>(async (textBox) =>
                {
                    if (!string.IsNullOrEmpty(textBox.Text))   //进行排查
                    {
                        Search(textBox.Text);
                    } 
                    else    //显示全部
                    {
                        Administrators = await serverAgent.GetAdministrators();
                    }
                });
            }
        }

       

        #endregion
    }
}
