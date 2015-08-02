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
            foreach (String slice in Splitter.SplitBySentence(userText))
            {   
                String r = findPattern(dbText, slice);
                var indexes = r.Split(';').Where(x => x != "").Select(x => Convert.ToInt32(x)).ToList();
                foreach (int i in indexes)
                {
                    interpreter.addIndex(resultIndexId, sliceIndex, sliceIndex + slice.Length, i, i + slice.Length);
                    resultIndexId++;
                   // Console.Out.WriteLine();
                    //Console.Out.WriteLine(slice);
                }
                sliceIndex = sliceIndex + slice.Length + 2;
            }
            
            interpreter.getNormalizedIndexes();
            return interpreter;
        }

    }
}
