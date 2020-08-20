using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CSharpCrawler.Util
{
    public class ImageUtil
    {
        public static bool SaveControlContentAsImage(System.Windows.FrameworkElement element,string fileName,int imgWidth = 0,int imgHeight = 0)
        {
            try
            {
                if (imgWidth == 0)
                    imgWidth = (int)element.ActualWidth;

                if (imgHeight == 0)
                    imgHeight = (int)element.ActualHeight;

                var render = new RenderTargetBitmap(imgWidth, imgHeight, 96, 96, PixelFormats.Default);
                render.Render(element);
                //暂时默认使用jpg
                BitmapEncoder encoder = new JpegBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(render));
                using (System.IO.FileStream fs = new System.IO.FileStream(fileName, System.IO.FileMode.OpenOrCreate))
                {
                    encoder.Save(fs);
                }
                return true;
            }
            catch
            {
                return false;
            }
        } 
    }
}
