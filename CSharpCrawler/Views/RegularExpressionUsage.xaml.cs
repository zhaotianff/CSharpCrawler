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
using System.Text.RegularExpressions;

namespace CSharpCrawler.Views
{
    /// <summary>
    /// RegularExpressionUsage.xaml 的交互逻辑
    /// </summary>
    public partial class RegularExpressionUsage : Page
    {
        public RegularExpressionUsage()
        {
            InitializeComponent();
            InitContent();
        }

        private void InitContent()
        {
            this.rtbx_Content.Document = new FlowDocument(new Paragraph(new Run(Properties.Resources.RegexHtml)));
        }

        private void btn_Match_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                TextRange tr = new TextRange(this.rtbx_Content.Document.ContentStart, rtbx_Content.Document.ContentEnd);

                string input = tr.Text;
                string pattern = this.tbox_Pattern.Text;
                List<dynamic> matchList = new List<dynamic>();
                List<dynamic> groupList = new List<dynamic>();

                MatchCollection matches = Regex.Matches(input, pattern, RegexOptions.IgnoreCase);
                if (matches.Count > 0)
                {
                    this.tbox_MatchStatus.Text = "匹配成功";

                    for (int i = 0; i < matches.Count; i++)
                    {
                        matchList.Add(new { Id = i + 1, Result = matches[i].Value });

                        GroupCollection groups = matches[i].Groups;

                        for (int j = 0; j < groups.Count; j++)
                        {
                            groupList.Add(new { Id = j + 1, Result = groups[j].Value });
                        }
                    }

                    this.listview_MatchResult.ItemsSource = matchList;
                    this.listview_GroupResult.ItemsSource = groupList;
                }
                else
                {
                    this.tbox_MatchStatus.Text = "匹配不成功";
                }
            }
            catch(Exception ex)
            {
                this.tbox_MatchStatus.Text = "程序异常，异常信息:" + ex.Message;
            }
        }
    }
}
