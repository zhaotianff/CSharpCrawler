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

namespace CSharpCrawler.Controls
{
    public class ListImage : Control
    {
        static ListImage()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ListImage), new FrameworkPropertyMetadata(typeof(ListImage)));
        }
      
        public static readonly DependencyProperty ImageProperty = DependencyProperty.Register("Image", typeof(BitmapImage), typeof(FrameworkElement));
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(FrameworkElement));
        public static readonly DependencyProperty ImageTypeProperty = DependencyProperty.Register("ImageType", typeof(string), typeof(FrameworkElement));

        public BitmapImage Image
        {
            set
            {
                SetValue(ImageProperty, value);
            }
            get
            {
                return (BitmapImage)GetValue(ImageProperty);
            }
        }

        public string Text
        {
            set
            {
                SetValue(TextProperty, value);
            }
            get
            {
                return (string)GetValue(TextProperty);
            }
        }

        public string ImageType
        {
            set
            {
                SetValue(ImageTypeProperty, value);
            }

            get
            {
                return (string)GetValue(ImageTypeProperty);
            }
        }
    }
}
