using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Linq;
using System.IO;

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
            object obj = xmlSerializer.Deserialize(stream);
            stream.Dispose();
            return (T)obj;
        }

    }
}
