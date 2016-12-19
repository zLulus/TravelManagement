using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ModernUINavigation.Pages.Settings
{
    /// <summary>
    /// Interaction logic for About.xaml
    /// </summary>
    public partial class About : UserControl
    {
        public About()
        {
            InitializeComponent();
            ContentTextBlock.Text = @"      畅游无忧——“智慧旅游”APP项目研制开发的意义在于以“安全”“资讯”“分享”“交友”多个角度去践行智慧旅游的倡议。智慧旅游建设将有利于优化旅游环境，提高各景区管理服务水平，确保旅游生态效益、经济效益和社会效益三者之间的统筹协调发展；并极大地丰富景区的管理手段和营销手段，为现代新旅游、新传播、新行为、新市场、新模式提供高科技服务，并将游戏性的吸引力和亲和力融为一体，将生态环境保护、景区安全领域优化整合，使其成为一种新的科技旅游，提升景区的品牌形象和社会形象。
      智慧旅游有助于提升风景区的核心吸引力和亲和力，为景区创造更高的经济效益。";
        }
    }
}
