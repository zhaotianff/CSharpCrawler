using BerkeleyDB;
using CSharpCrawler.Util;
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
        private const string DBPath = "demo.db";

        BDBHelper bdb;

        public DataStorage()
        {
            //更详细的使用方式，可以访问
            //https://www.oracle.com/database/technologies/related/berkeleydb-downloads.html
            //下载源码包，里面有C#使用示例
            //这里只是介绍基本使用
            InitializeComponent();

            bdb = new BDBHelper(DBPath);
        }

        #region Oracle BDB
        private void btn_WriteStringToBDB_Click(object sender, RoutedEventArgs e)
        {
            string guid = this.tbox_Key.Text;
            string url = this.tbox_Value.Text;
            DatabaseEntry key = new DatabaseEntry();
            key.Data = Encoding.ASCII.GetBytes(guid);
            DatabaseEntry value = new DatabaseEntry();
            value.Data = Encoding.ASCII.GetBytes(url);
            bdb.Put(key, value);
            EMessageBox.Show("写入成功");       
        }

        private void btn_ReadStringFromBDB_Click(object sender, RoutedEventArgs e)
        {
            var guid = this.tbox_Key.Text;
            var key = new DatabaseEntry();
            key.Data = Encoding.ASCII.GetBytes(guid);
            KeyValuePair<DatabaseEntry, DatabaseEntry> pair = bdb.Get(key);
            EMessageBox.Show(Encoding.ASCII.GetString(pair.Value.Data));
        }

        private void btn_WriteObjectToBDB_Click(object sender, RoutedEventArgs e)
        {
            var key = new DatabaseEntry();
            key.Data = Encoding.ASCII.GetBytes("TestClassKey");
            CrawlerRecord crawlerRecord = new CrawlerRecord() { id = 1,content = "<html><body><h1>Hello World</h1></body></html>",title = "helloworld",url = "https://myfreetime.cn"};
            var data = new DatabaseEntry();
            data.Data = crawlerRecord.GetBytes();
            //写入操作
            bdb.Put(key, data);
            EMessageBox.Show("写入成功");
        }

        private void btn_ReadObjectFromBDB_Click(object sender, RoutedEventArgs e)
        {
            var key = new DatabaseEntry();
            key.Data = Encoding.ASCII.GetBytes("TestClassKey");
            CrawlerRecord record = bdb.Get<CrawlerRecord>(key);

            if(record != null)
            {
                EMessageBox.Show($"ID = {record.id},Url = {record.url},Title = {record.title},Content = {record.content}");
            }
        }

        private void btn_WriteImageToBDB_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog fileDialog = new Microsoft.Win32.OpenFileDialog();
            fileDialog.Filter = "图像文件|*.jpg;*.png;*.bmp;|全部文件|*.*";
            fileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            if (fileDialog.ShowDialog().Value == true)
            {
                BitmapImage bi = new BitmapImage();
                bi.BeginInit();
                bi.UriSource = new Uri(fileDialog.FileName);
                bi.CacheOption = BitmapCacheOption.OnLoad;
                bi.EndInit();
                this.img.Source = bi;

                var key = new DatabaseEntry();
                key.Data = Encoding.ASCII.GetBytes("TestImageKey");
                bdb.Put(key, fileDialog.FileName);

                EMessageBox.Show("写入成功");
            }
        }

        private void btn_ReadImageFromBDB_Click(object sender, RoutedEventArgs e)
        {
            var key = new DatabaseEntry();
            key.Data = Encoding.ASCII.GetBytes("TestImageKey");
            var pair = bdb.Get(key);

            //可以直接保存文件
            //System.IO.File.WriteAllBytes("test.jpg", pair.Value.Data);
       
            //显示在界面上
            BitmapImage bi = new BitmapImage();
            System.IO.MemoryStream ms = new MemoryStream(pair.Value.Data);          
            bi.BeginInit();
            bi.CacheOption = BitmapCacheOption.OnLoad;
            bi.StreamSource = ms;
            bi.EndInit();
            this.img.Source = bi;
            ms.Close();
        }
        #endregion

        #region SQLite
        private void btnAddParameters_Click(object sender, RoutedEventArgs e)
        {
            CSharpCrawler.UserControls.SQLiteParameterUI sQLiteParameterUI = new UserControls.SQLiteParameterUI();
            this.stackPanel_Parameters.Children.Add(sQLiteParameterUI);
        }

        private void btnRemoveParameters_Click(object sender, RoutedEventArgs e)
        {

        }
        #endregion

        #region MongoDB

        #endregion

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
