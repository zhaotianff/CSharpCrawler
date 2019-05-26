using BerkeleyDB;
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
    /// DataStorage.xaml 的交互逻辑
    /// </summary>
    public partial class DataStorage : Page
    {
        public DataStorage()
        {
            InitializeComponent();
        }

        private void btn_WriteToBDB_Click(object sender, RoutedEventArgs e)
        {
            BTreeDatabaseConfig bTreeDatabaseConfig = new BTreeDatabaseConfig();
            //文件不存在则创建
            bTreeDatabaseConfig.Creation = CreatePolicy.IF_NEEDED;
            //页大小
            bTreeDatabaseConfig.PageSize = 512;
            //缓存大小
            bTreeDatabaseConfig.CacheSize = new CacheInfo(0, 64 * 1024, 1);
            BTreeDatabase bTreeDatabase = BTreeDatabase.Open("demo.db", bTreeDatabaseConfig);
            string url = this.tbox_Url.Text;
            DatabaseEntry key = new DatabaseEntry(Encoding.ASCII.GetBytes(url));
            DatabaseEntry value = new DatabaseEntry(BitConverter.GetBytes(1));
            bTreeDatabase.Put(key, value);
            EMessageBox.Show("写入成功");
            KeyValuePair<DatabaseEntry,DatabaseEntry> pair = bTreeDatabase.Get(key);
            EMessageBox.Show(Encoding.ASCII.GetString( pair.Key.Data));
            bTreeDatabase.Close();
            
        }
    }
}
