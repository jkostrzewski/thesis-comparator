using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestApp1.Result;

namespace TestApp1.Algorithm
{
    class SentenceSplit : CompareAlgorithm
    {
        
        public override ResultInterpreterOpt check(ResultInterpreterOpt interpreter, String dbText, String userText)
        {
            int sliceIndex = 0;
            int resultIndexId = 0;
            foreach (String slice in SplitBySentence(userText))
            {   
                String r = findPattern(dbText, slice);
                var indexes = r.Split(';').Where(x => x != "").Select(x => Convert.ToInt32(x)).ToList();
                foreach (int i in indexes)
                {
                    ResultIndex resultIndex = new ResultIndex(resultIndexId, sliceIndex, sliceIndex + slice.Length, i, i + slice.Length);
                    resultIndexId++;
                   // Console.Out.WriteLine();
                    //Console.Out.WriteLine(slice);
                    interpreter.addIndex(resultIndex);
                }
                sliceIndex = sliceIndex + slice.Length + 2;
            }
            
            interpreter.getNormalizedIndexes();
            return interpreter;
        }

        public IEnumerable<string> SplitBySentence(string str)
        {
            string[] stringSeparators = new string[] {". "};
            String[] blacklist = {""};
            foreach (String sentence in str.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries))
            {
                if (!blacklist.Contains(sentence)) {
                    //Console.Out.WriteLine(sentence);
                    yield return sentence;
                }
                else
                {
                    Console.Out.WriteLine("|"+sentence+"|");
                    continue;
                }
            }
        }
    }
}
