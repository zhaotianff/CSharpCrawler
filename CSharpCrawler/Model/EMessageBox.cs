using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZT.Enhance
{
    class EMessageBox
    {
        public static void Show(string content)
        {
            ISeer.GUI.MessageUI message = new ISeer.GUI.MessageUI(content);
            message.ShowDialog();
        }

        public static void Show(string content,string title)
        {
            ISeer.GUI.MessageUI message = new ISeer.GUI.MessageUI(content,title);
            message.ShowDialog();
        }

        public static bool? Show(string content,string tile,ISeer.Utilities.EMessageBoxType.ButtonType type)
        {
            ISeer.GUI.MessageUI message = new ISeer.GUI.MessageUI(content,tile,type);
            message.ShowDialog();
            return message.DialogResult;
        }
    }
}
