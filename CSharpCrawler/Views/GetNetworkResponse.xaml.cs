using CSharpCrawler.Model;
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

namespace CSharpCrawler.Views
{
    /// <summary>
    /// GetNetworkResponse.xaml 的交互逻辑
    /// </summary>
    public partial class GetNetworkResponse : Page
    {
        public GetNetworkResponse()
        {
            InitializeComponent();
        }

        private void btn_Fetch_Click(object sender, RoutedEventArgs e)
        {
            this.list.Items.Clear();

            GlobalDataUtil.GetInstance().Browser.GetNetworkResponseDynamic(this.tbox_Url.Text, GetNetworkResponseAction);
        }

        public void GetNetworkResponseAction(NetworkResponse networkResponse)
        {
            this.Dispatcher.Invoke(() => {
                this.list.Items.Add(networkResponse);
            });
        }
    }
}
