using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp1.Algorithm
{
    class Splitter
    {
        public static IEnumerable<string> SplitBySentence(string str)
        {
            string[] stringSeparators = new string[] { ". " };
            String[] blacklist = { "" };
            foreach (String sentence in str.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries))
            {
                if (!blacklist.Contains(sentence))
                {
                    //Console.Out.WriteLine(sentence);
                    yield return sentence;
                }
                else
                {
                    Console.Out.WriteLine("|" + sentence + "|");
                    continue;
                }
            }
        }

        public static IEnumerable<string> SplitByNGram(string str, int n)
        {
            string[] stringSeparators = new string[] { " " };
            String[] blacklist = { "", " " };
            var splitedString = str.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries).ToList<String>();
            for (int i = 0; i < splitedString.Count - n + 1; i++)
            {
                yield return string.Join(" ", splitedString.GetRange(i, n));

            }

        }

        public static IEnumerable<string> SplitByLength(string str, int width)
        {
            for (int index = 0; index < str.Length; index += width)
            {
                yield return str.Substring(index, Math.Min(width, str.Length - index));
            }
        }

        public static IEnumerable<string> SplitByWindow(string str, int width)
        {
            for (int index = 0; index < str.Length - width; index++)
            {
                yield return str.Substring(index, Math.Min(width, str.Length - index));
            }
        }
    }
}
