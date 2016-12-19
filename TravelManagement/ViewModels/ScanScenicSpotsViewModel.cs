using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using SimpleMvvmToolkit;
// Toolkit namespace

namespace TravelManagement.ViewModels
{
    /// <summary>
    ///     This class contains properties that a View can data bind to.
    ///     <para>
    ///         Use the <strong>mvvmprop</strong> snippet to add bindable properties to this ViewModel.
    ///     </para>
    /// </summary>
    /// 景点输入管理
    public class ScanScenicSpotsViewModel : ViewModelBase<ScanScenicSpotsViewModel>
    {
        private Window DialogScan;
        private string name;

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                NotifyPropertyChanged(m => m.Name);
            }
        }

        public ICommand LoadWindowCommand
        {
            get { return new DelegateCommand<Window>(dialog => { DialogScan = dialog; }); }
        }

        public ICommand SaveCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    if (((TextBox) DialogScan.FindName("TxtName")).Text.Trim() == "" ||
                        ((TextBox) DialogScan.FindName("TxtDescription")).Text.Trim() == "")
                    {
                        MessageBox.Show("景点信息不能为空！", "警告");
                        return;
                    }
                    DialogScan.Close();
                });
            }
        }

        public ICommand CloseCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    Name = null;   //为了主界面判断正确
                    DialogScan.Close();
                });
            }
        }
    }
}