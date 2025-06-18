using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVA.AddOn.Common.Util
{
    public class StringUtil
    {
        public static string RemoveAccents(string value)
        {
            byte[] bytes = Encoding.GetEncoding("Cyrillic").GetBytes(value);
            return Encoding.ASCII.GetString(bytes);
        }
    }
}
