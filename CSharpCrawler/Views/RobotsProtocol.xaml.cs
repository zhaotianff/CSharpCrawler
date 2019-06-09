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
using System.IO;

namespace CSharpCrawler.Views
{
    /// <summary>
    /// RobotsProtocol.xaml 的交互逻辑
    /// </summary>
    public partial class RobotsProtocol : Page
    {
        private bool hasLoaded = false;

        public RobotsProtocol()
        {
            InitializeComponent();
        }

        public void LoadContent()
        {
            if (hasLoaded)
                return;

            try
            {
                using (FileStream fs = File.Open("../../../RobotsExclusionProtocol.txt", FileMode.Open))
                {
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        this.lbl_Content.Document = new FlowDocument(new Paragraph(new Run(sr.ReadToEnd()))); 
                    }
                }

                hasLoaded = true;
            }
            catch(Exception ex)
            {
                this.lbl_Content.Document = new FlowDocument(new Paragraph(new Run(ex.Message))); 
            }
        }
    }
}
