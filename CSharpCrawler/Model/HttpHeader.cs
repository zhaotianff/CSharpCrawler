using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CSharpCrawler.Model
{
    class HttpHeader
    {
        public string ContentEncoding { get; set; }

        public long ContentLength { get; set; }

        public string ContentType { get; set; }

        public DateTime LastModified { get; set; }

        public string Server { get; set; }

        public string CharSet { get; set; }

        public HttpStatusCode StatusCode { get; set; }
    }
}
