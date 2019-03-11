using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CSharpCrawler.Util
{
    public class XmlUtil<T>
    {
        //private T type = default(T);

        public XmlUtil()
        {
            
        }

        public T DeserializeXML(System.IO.Stream stream)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            return (T)xmlSerializer.Deserialize(stream);
        }

    }
}
