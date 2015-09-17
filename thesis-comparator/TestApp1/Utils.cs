using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TestApp1
{
    class Utils
    {

        public static String normalizeText(String inText)
        {
            String outText = inText;
            outText = outText.ToLower();
            outText = Regex.Replace(outText, "[^a-zA-Z0-9_.()!@#$%^&*[] ]+", "", RegexOptions.Compiled);
            outText = Regex.Replace(outText, "[ ]+", " ", RegexOptions.Compiled);
            outText = Regex.Replace(outText, "[\n]+", "\n", RegexOptions.Compiled);
            return outText;
        }

        public static String normalizeDbText(String inText)
        {
            String outText = inText.ToLower();
            outText = Regex.Replace(outText, "[ ]+", " ", RegexOptions.Compiled);
            outText = Regex.Replace(outText, "[\r\n]+", " ", RegexOptions.Compiled);
            outText = Regex.Replace(outText, "[^a-zA-Z0-9.():,?!+ -]+", "", RegexOptions.Compiled);
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
