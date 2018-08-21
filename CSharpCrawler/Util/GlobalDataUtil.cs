using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpCrawler.Util
{
    class GlobalDataUtil
    {
        private static object obj = new object();
        private static GlobalDataUtil _instance;

        

        static GlobalDataUtil GetInstance()
        {
            if(_instance == null)
            {
                lock(obj)
                {
                    if (_instance == null)
                        _instance = new GlobalDataUtil();
                }
            }
            return _instance;
        }

        public GlobalDataUtil()
        {

        }
    }
}
