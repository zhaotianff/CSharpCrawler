using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpCrawler.Model
{
    public class UrlStruct : INotifyPropertyChanged
    {
        private int id;
        private string url;
        private string title;
        private string status;

        public int Id
        {
            get
            {
                return id;
            }

            set
            {
                id = value;
                RaiseChange("Id");
            }
        }

        public string Url
        {
            get
            {
                return url;
            }

            set
            {
                url = value;
                RaiseChange("Url");
            }
        }

        public string Title
        {
            get
            {
                return title;
            }

            set
            {
                title = value;
                RaiseChange("Title");
            }
        }

        public string Status
        {
            get
            {
                return status;
            }

            set
            {
                status = value;
                RaiseChange("Status");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaiseChange(string property)
        {
            if(PropertyChanged != null)
            {
                PropertyChanged.Invoke(this,new PropertyChangedEventArgs(property));
            }
        }
    }
}
