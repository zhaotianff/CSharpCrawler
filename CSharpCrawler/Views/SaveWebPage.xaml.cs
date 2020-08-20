using CSharpCrawler.Util;
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
using ZT.Enhance;

namespace CSharpCrawler.Views
{
    /// <summary>
    /// SaveWebPage.xaml 的交互逻辑
    /// </summary>
    public partial class SaveWebPage : Page
    {
        public SaveWebPage()
        {
            InitializeComponent();
        }


        private void btn_SaveAsImage_Click(object sender, RoutedEventArgs e)
        {
            var fileName = Environment.CurrentDirectory + "\\download\\web.jpg";
        }

        private void SaveByRenderTargetBitmap()
        {
            
            var result = ImageUtil.SaveControlContentAsImage(this, fileName);


    }
}
