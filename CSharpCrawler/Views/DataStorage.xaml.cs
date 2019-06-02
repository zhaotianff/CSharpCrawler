using BerkeleyDB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
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

        private void btn_WriteStringToBDB_Click(object sender, RoutedEventArgs e)
        {
            BTreeDatabaseConfig bTreeDatabaseConfig = new BTreeDatabaseConfig();
            //文件不存在则创建
            bTreeDatabaseConfig.Creation = CreatePolicy.IF_NEEDED;
            //页大小
            bTreeDatabaseConfig.PageSize = 512;
            //缓存大小
            bTreeDatabaseConfig.CacheSize = new CacheInfo(0, 64 * 1024, 1);
            BTreeDatabase bTreeDatabase = BTreeDatabase.Open("demo.db", bTreeDatabaseConfig);
            string guid = this.tbox_Key.Text;
            string url = this.tbox_Value.Text;
            DatabaseEntry key = new DatabaseEntry();
            key.Data = Encoding.ASCII.GetBytes(guid);
            DatabaseEntry value = new DatabaseEntry();
            value.Data = Encoding.ASCII.GetBytes(url);
            bTreeDatabase.Put(key, value);
            EMessageBox.Show("写入成功");
            bTreeDatabase.Close();         
        }

        private void btn_ReadStringFromBDB_Click(object sender, RoutedEventArgs e)
        {
            //KeyValuePair<DatabaseEntry, DatabaseEntry> pair = bTreeDatabase.Get(key);
            //EMessageBox.Show(Encoding.ASCII.GetString(pair.Key.Data));
        }

        private void btn_WriteObjectToBDB_Click(object sender, RoutedEventArgs e)
        {
            var key = new DatabaseEntry();
            key.Data = Encoding.ASCII.GetBytes(Guid.NewGuid().ToString());
            CrawlerRecord crawlerRecord = new CrawlerRecord() { id = 1,content = "<html><body><h1>Hello World</h1></body></html>",title = "helloworld",url = "https://myfreetime.cn"};
            var data = new DatabaseEntry();
            data.Data = crawlerRecord.GetBytes();
            //写入操作
        }

        private void btn_ReadObjectFromBDB_Click(object sender, RoutedEventArgs e)
        {

        }
    }

    [Serializable]
    class CrawlerRecord
    {
        public int id;
        public string url;
        public string title;
        public string content;

        public byte[] GetBytes()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream memStream = new MemoryStream();
            formatter.Serialize(memStream, this);
            byte[] bytes = memStream.GetBuffer();
            memStream.Close();
            return bytes;
        }
    }
}
