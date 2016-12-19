using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using SimpleMvvmToolkit;

namespace TravelManagement.Models
{
    public class Model : ModelBase<Model>
    {
        private string title;
        private ImageSource modelImage;

        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                NotifyPropertyChanged(m => m.Title);
            }
        }

        public ImageSource ModelImage
        {
            get { return modelImage; }
            set
            {
                modelImage = value;
                NotifyPropertyChanged(m => m.ModelImage);
            }
        }
    }
}
