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

namespace CSharpCrawler.Views
{
    /// <summary>
    /// BingImageSearch.xaml 的交互逻辑
    /// </summary>
    public partial class BingImageSearch : Page
    {
        DoubleAnimation startAnimation;

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
        }

        public void StartAnimation()
        {
            this.grid.BeginAnimation(Canvas.LeftProperty,startAnimation);
        }
    }
}
