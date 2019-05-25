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
    /// Basic.xaml 的交互逻辑
    /// </summary>
    public partial class Basic : Page
    {
        public Basic()
        {
            InitializeComponent();
        }

        private void btn_Fetch_Click(object sender, RoutedEventArgs e)
        {
            string hostName = this.tbox_Url.Text.Trim();
            if(string.IsNullOrEmpty(hostName))
            {
                EMessageBox.Show("请输入主机名，如www.baidu.com");
                return;
            }          

            try
            {
                string content = WebUtil.GetHtmlSourceWithSocket(hostName);
                this.rtbox_Content.Document = new FlowDocument(new Paragraph(new Run(content)));
            }
            catch(Exception ex)
            {
                ShowStatusMessage(ex.Message);
            }
        }

        private void ShowStatusMessage(string msg)
        {
            this.Dispatcher.Invoke(()=> { this.lbl_Status.Content = msg; });
        }
    }
}
