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

namespace CSharpCrawler.UserControls
{
    /// <summary>
    /// SQLiteParameterUI.xaml 的交互逻辑
    /// </summary>
    public partial class SQLiteParameterUI : UserControl
    {
        private static readonly Dictionary<string, Type> SQLiteDataTypes = new Dictionary<string, Type>()
        {
            ["NULL"] = null,
            ["INTEGER"] = typeof(int),
            ["REAL"] = typeof(float),
            ["TEXT"] = typeof(string),
            ["BLOB"] = typeof(object)
        };

        public string ParameterName { get => this.tbox_ParameterName.Text; }
        public Type ParameterType { get => SQLiteDataTypes[this.combox_DataTypes.SelectedItem.ToString()]; }

        public SQLiteParameterUI()
        {
            InitializeComponent();
            this.combox_DataTypes.ItemsSource = SQLiteDataTypes.Keys.ToList();
            this.combox_DataTypes.SelectedIndex = 3;
        }
    }
}
