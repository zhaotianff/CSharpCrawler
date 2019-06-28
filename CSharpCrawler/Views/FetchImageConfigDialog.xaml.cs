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

        public FetchImageConfigDialog()
        {
            InitializeComponent();

            end = this.TryFindResource("end") as Storyboard;
            if(end != null)
            {
                end.Completed += (a, b) => { this.Close(); };
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
    }
}
