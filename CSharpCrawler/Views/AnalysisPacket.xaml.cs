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
    /// AnalysisPacket.xaml 的交互逻辑
    /// </summary>
    public partial class AnalysisPacket : Page
    {
#if DEBUG
        private const string FiddlerMarkDownPath = "../../../AnalysisPacket_Fiddler.md";
        private const string FiddlerTempHtmlPath = "../../../AnalysisPacket_Fiddler.html";
#else
        private const string FiddlerMarkDownPath = "./AnalysisPacket_Fiddler.md";
        private const string FiddlerTempHtmlPath = "./AnalysisPacket_Fiddler.html";
#endif

        public AnalysisPacket()
        {
            InitializeComponent();
        }

        public void LoadContent()
        {
            //使用MarkDig将目录下的markdown转成html显示在界面上
            var fiddlerMarkDownPath = FiddlerMarkDownPath;
            var fullPath = System.IO.Path.GetFullPath(fiddlerMarkDownPath);
            if (!System.IO.File.Exists(fullPath))
            {
                EMessageBox.Show("Markdown文件不存在");
                return;
            }
            var markdown = System.IO.File.ReadAllText(fullPath);
            var html = Markdig.Markdown.ToHtml(markdown);

            //CEF不支持直接设置网页内容，只能保存成文件
            if(!System.IO.File.Exists(FiddlerTempHtmlPath))
            {
                System.IO.File.WriteAllText(FiddlerTempHtmlPath, html,Encoding.UTF8);
            }

            
            this.chrome.Address = "file:///" + System.IO.Path.GetFullPath( FiddlerTempHtmlPath);
        }
    }
}
