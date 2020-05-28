using CSharpCrawler.Model;
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
using System.Windows.Shapes;
using ZT.Enhance;

namespace CSharpCrawler.Views
{
    /// <summary>
    /// Interaction logic for FetchImageConfigDialog.xaml
    /// </summary>
    public partial class FetchImageConfigDialog : Window
    {
        Storyboard end;
        Storyboard start;

        public double X
        {
            get
            {
                return this.scaleTransform.CenterX;
            }
            set
            {
                this.scaleTransform.CenterX = value;
            }
        }

        public double Y
        {
            get
            {
                return this.scaleTransform.CenterY;
            }
            set
            {
                this.scaleTransform.CenterY = value;
            }
        }

        public void Init(PageDownRuleType pageDownRule,PageDownMethodType pageDownMethod,string pageDownUrl,string postData)
        {
            if(pageDownRule == PageDownRuleType.Auto)
            {
                this.cbx_AutoRule.IsChecked = true;
            }
            else
            {
                this.cbx_ManualRule.IsChecked = true;
                this.tbox_url.Text = pageDownUrl;

                if (pageDownMethod == PageDownMethodType.Url)
                {
                    this.cbx_url.IsChecked = true;
                }
                else
                {
                    this.cbx_post.IsChecked = true;
                    this.tbox_postdata.Text = postData;
                }
            }        
        }

        public FetchImageConfigDialog()
        {
            InitializeComponent();

            end = this.TryFindResource("end") as Storyboard;
            if(end != null)
            {
                end.Completed += (a, b) => { this.DialogResult = false; };
            }
            start = this.TryFindResource("start") as Storyboard;
        }

        private void ShowEndAnimation()
        {
            if (end != null)
                this.BeginStoryboard(end);
        }

        private void ShowStartAnimation()
        {          
            if (start != null)
                this.BeginStoryboard(start);
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Main_Loaded(object sender, RoutedEventArgs e)
        {
            ShowStartAnimation();
        }

        private void Btn_Cancel_Click(object sender, RoutedEventArgs e)
        {         
            ShowEndAnimation();
        }

        private void cbx_AutoRule_Checked(object sender, RoutedEventArgs e)
        {
            cbx_ManualRule.IsChecked = false;
        }

        private void cbx_ManualRule_Checked(object sender, RoutedEventArgs e)
        {
            cbx_AutoRule.IsChecked = false;
        }

        private void cbx_url_Checked(object sender, RoutedEventArgs e)
        {
            cbx_post.IsChecked = false;
        }

        private void cbx_post_Checked(object sender, RoutedEventArgs e)
        {
            cbx_url.IsChecked = false;
        }

        private void btn_OK_Click(object sender, RoutedEventArgs e)
        {
            EMessageBox.Show("当前配置已生效，但还不会写入配置文件");
            this.DialogResult = true;
        }
    }
}
