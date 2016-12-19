using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using FirstFloor.ModernUI.Presentation;
using SimpleMvvmToolkit;
using TravelManagement.Views;
using System.Windows.Controls;

namespace TravelManagement.ViewModels
{
    public class MainWindowViewModel:ViewModelBase<MainWindowViewModel>
    {
        private string titleText;
        private Visibility visibility;
        private Visibility titleVisibility;

        /// <summary>
        /// 标题
        /// </summary>
        public Visibility TitleVisibility
        {
            get { return titleVisibility; }
            set
            {
                titleVisibility = value;
                NotifyPropertyChanged(m => m.TitleVisibility);
            }
        }

        /// <summary>
        /// 返回
        /// </summary>
        public Visibility Visibility
        {
            get { return visibility; }
            set
            {
                visibility = value;
                NotifyPropertyChanged(m => m.Visibility);
            }
        }

        /// <summary>
        /// 顶部返回键旁边的文本
        /// </summary>
        public string TitleText
        {
            get { return titleText; }
            set
            {
                titleText = value;
                NotifyPropertyChanged(m => m.TitleText);
            }
        }

        public void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Global.MainWindow = sender as MainWindow;
            Global.ViewStack.Push(new ModelsUserControl());  //加一个新实例即可，因为没有“操作记录”
            Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// 返回键
        /// </summary>
        public ICommand GoBackCommand
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    if (Global.ViewStack.Count == 1)
                    {
                        return;
                    }
                    Global.ViewStack.Pop();
                    Global.CurrView = Global.ViewStack.Peek();
                    Global.MainWindow.ViewContiner.Children.Clear();
                    Global.MainWindow.ViewContiner.Children.Add(Global.CurrView);
                    Global.ViewStack.Push(Global.CurrView);
                    //changeview又加了次，所以出栈
                    Global.ViewStack.Pop();
                    GoBackBtChange();
                });
            }
        }

        /// <summary>
        /// 隐藏or显示GoBack BT
        /// </summary>
        public void GoBackBtChange()
        {
            if (Global.ViewStack.Peek().GetType() == typeof(ModelsUserControl))
            {
                Visibility = Visibility.Collapsed;
                TitleVisibility= Visibility.Visible;
                TitleText = "";
            }
            else
            {
                Visibility = Visibility.Visible;
                TitleVisibility = Visibility.Collapsed;
                //todo 修改名称
                TitleText ="返回";
            }
        }
    }
}
