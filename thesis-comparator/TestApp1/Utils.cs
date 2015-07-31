using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp1
{
    class Utils
    {

        public static String normalizeText(String inText)
        {
            String outText = inText;
            outText = outText.ToLower();
            return outText;
        }

        public static String getDocumentFromFile(String path)
        {
            var s =System.IO.File.ReadAllText(path, Encoding.UTF8); 
             //System.IO.File.WriteAllText(path+"1", s, Encoding.UTF8);
             return s;
        }
    }
}
