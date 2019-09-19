using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpCrawler.Util
{
    public class EncryptionUtil
    {
        public static string MD5_32(string str)
        {
            byte[] strBytes = System.Text.Encoding.UTF8.GetBytes(str);
            strBytes = new System.Security.Cryptography.MD5CryptoServiceProvider().ComputeHash(strBytes);
            var strVal = "";
            for (int i = 0; i < strBytes.Length; i++)
            {
                strVal += strBytes[i].ToString("x").PadLeft(2, '0');
            }
            return strVal;
        }
    }
}
