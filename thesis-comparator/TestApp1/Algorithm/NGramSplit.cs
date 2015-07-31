using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestApp1.Result;

namespace TestApp1.Algorithm
{
    class NGramSplit : CompareAlgorithm
    {

        public override ResultInterpreterOpt check(ResultInterpreterOpt interpreter, String dbText, String userText)
        {
            int n = 3;
            int sliceIndex = 0;
            int resultIndexId = 0;
            foreach (String slice in SplitByNGram(userText, n))
            {
                if (slice == "")
                {
                    sliceIndex++;
                    continue;
                }
               // Console.Out.WriteLine(slice);
               // Console.Out.WriteLine(dbText.Substring(sliceIndex, 30));
               // Console.Out.WriteLine("|");
                //Console.ReadLine();
                String r = findPattern(dbText, slice);
                
                var indexes = r.Split(';').Where(x => x != "").Select(x => Convert.ToInt32(x)).ToList();
                //Console.Out.WriteLine(indexes.Count)
                int skipValue = slice.Split(' ')[0].Length;
                foreach (int i in indexes)
                {
                    //Console.Out.WriteLine(slice);
                    ResultIndex resultIndex = new ResultIndex(resultIndexId, sliceIndex + 1, sliceIndex + slice.Length+1 , i , i + slice.Length );
                    resultIndexId++;
                    // Console.Out.WriteLine();
                    //Console.Out.WriteLine(slice);
                    interpreter.addIndex(resultIndex);
                }
                sliceIndex = sliceIndex + skipValue + 1;
            }
            Console.Out.WriteLine("found");
            interpreter.getNormalizedIndexes();
            Console.Out.WriteLine("normalized");
            return interpreter;
        }

        public IEnumerable<string> SplitByNGram(string str, int n)
        {
            string[] stringSeparators = new string[] {" "};
            String[] blacklist = { "", " " };
            var splitedString = str.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries).ToList<String>();
            for (int i = 0; i < splitedString.Count - n+1; i++)
            {
                //Console.Out.WriteLine(n);
                //Console.Out.WriteLine(string.Join(" ", splitedString.GetRange(0, n)));
                //Console.Out.WriteLine(string.Join(" ", splitedString.GetRange(i, n)));
               // Console.Out.WriteLine(string.Join(" ", splitedString.GetRange(0, 20)));
                
               // Console.ReadLine();
                yield return string.Join(" ", splitedString.GetRange(i, n));

            }

        }
    }
}
