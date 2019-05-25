using CSharpCrawler.Model;
using CSharpCrawler.Util;
using CSharpCrawler.Controls;
using GetImageZUI.Views;

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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ZT.Enhance;

namespace CSharpCrawler.Views
{
    /// <summary>
    /// BingImageSearch.xaml 的交互逻辑
    /// </summary>
    public partial class BingImageSearch : Page
    {
        DoubleAnimation startAnimation;
        WaitingDailog dialog;

        const int RowCount = 3;
        const int PageImageNum = 35;

        public BingImageSearch()
        {
            InitializeComponent();

            InitAnimation();
        }

        private void InitAnimation()
        {
            startAnimation = new DoubleAnimation();
            startAnimation.From = 10;
            startAnimation.To = 0;
            startAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(500));
            startAnimation.Completed += StartAnimation_Completed;
        }

        private void StartAnimation_Completed(object sender, EventArgs e)
        {
            LoadHotSpots();
        }

        private async void LoadHotSpots()
        {
            //不加载 
            if (this.grid_Content.Children.Count > 0)
                return;

            List<TagImg> hotSpotsImgList = new List<TagImg>();
            this.Dispatcher.BeginInvoke(new Action(()=> {
                dialog = new WaitingDailog("正在加载每日热图");
                dialog.ShowDialog();
            }));
         
            hotSpotsImgList = await HtmlAgilityPackUtil.GetImgFromUrl(UrlUtil.CNBingImageUrl,true);

            //去除
            hotSpotsImgList = hotSpotsImgList.Where(x => x.Src.Contains("tse1-mm")).ToList();

            //显示
            ShowImage(hotSpotsImgList,true);
            dialog.Close();
        }

        private async Task<List<TagImg>> SearchBingImage(string keyword,int page = 1)
        {
            List<TagImg> searchImgList = new List<TagImg>();
            var start = 1;
            if (page > 1)
                start = page * PageImageNum + 1;
            var url = UrlUtil.CNBingImageDetailUrl.Replace("[keyword]", keyword).Replace("[start]", start.ToString());
            searchImgList = await HtmlAgilityPackUtil.GetImgFromUrl(url);
            return searchImgList;
        }

        private void ShowImage(List<TagImg> imgList,bool isHotSpot = false)
        {
            Reset();

            int columns = imgList.Count / RowCount;
            int row = 0;

            if (imgList.Count % RowCount != 0)
                columns++;

            for (int i = 0; i < columns; i++)
            {
                this.grid_Content.ColumnDefinitions.Add(new ColumnDefinition());
            }

            if(this.grid_Content.RowDefinitions.Count != RowCount)
            {
                for (int i = 0; i < RowCount; i++)
                {
                    this.grid_Content.RowDefinitions.Add(new RowDefinition());
                }
            }

            //暂不做异步加载
            for(int i = 0;i<imgList.Count;i++)
            {
                string detailUrl = imgList[i].DetailUrl;

                TextImage image = new TextImage();
                image.Width = 300;
                image.Margin = new Thickness(5);
                image.Image = imgList[i].Src;
                image.Text = imgList[i].Alt;
                image.Cursor = Cursors.Hand;
                if(isHotSpot)
                {
                    image.MouseDown += (a, b) => { this.tbox_Keyword.Text = image.Text;btn_Search_Click(null, null); };
                }
                else
                {                   
                    image.MouseDown +=(a, b) =>
                    {
                        string imgUrl = detailUrl;
                        Point p = b.GetPosition(Application.Current.MainWindow);
                        AnimationImageWindow animationDialog = new AnimationImageWindow();                      
                        animationDialog.ShowImage(imgUrl, p.X, p.Y);
                    };
                }
                Grid.SetColumn(image, i / RowCount);
                Grid.SetRow(image, row);
                this.grid_Content.Children.Add(image);

                row++;
                if (row == RowCount)
                    row = 0;
            }          
        }

        private void Reset()
        {
            this.grid_Content.ColumnDefinitions.Clear();
        }

        public void StartAnimation()
        {
            this.grid.BeginAnimation(Canvas.LeftProperty,startAnimation);
        }

        private void scroll_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            //if (!(scroll.HorizontalScrollBarVisibility ==  ScrollBarVisibility.Hidden  || (scroll.HorizontalOffset == 0 && e.Delta > 0) || (scroll.HorizontalOffset == lastRightPanelVerticalScrollValue && e.Delta < 0)))
            //{

            //}
            if (e.Delta > 0)
            {
                scroll.ScrollToHorizontalOffset(scroll.HorizontalOffset - 50);
            }
            else
            {
                scroll.ScrollToHorizontalOffset(scroll.HorizontalOffset + 50);
            }
        }

        private async void btn_Search_Click(object sender, RoutedEventArgs e)
        {
            List<TagImg> searchResult = new List<TagImg>();
            string keyword = this.tbox_Keyword.Text.Trim();
            if(string.IsNullOrEmpty(keyword))
            {
                EMessageBox.Show("请输入搜索关键字");
                return;
            }

            searchResult =await SearchBingImage(keyword);
            ShowImage(searchResult);
        }
    }
}
