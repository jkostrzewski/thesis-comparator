using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestApp1.Result;

namespace TestApp1.Algorithm
{
    class EquiWidthSkip : CompareAlgorithm
    {
        private int width = 40;
        public override ResultInterpreterOpt check(ResultInterpreterOpt interpreter, String dbText, String userText)
        {
            
            int sliceIndex = 0;
            int resultIndexId = 0;
            foreach (String slice in Splitter.SplitByLength(userText, this.width))
            {                
                String r = findPattern(dbText, slice);
                var indexes = r.Split(';').Where(x => x != "").Select(x => Convert.ToInt32(x)).ToList();
                foreach (int i in indexes)
                {
                    //Console.Out.WriteLine(dbText.Substring(i, slice.Length));
                    ResultIndex resultIndex = new ResultIndex(resultIndexId, sliceIndex * slice.Length, (sliceIndex + 1) * slice.Length, i, i + slice.Length);
                    resultIndexId++;
                    interpreter.addIndex(resultIndex);
                }
                sliceIndex++;
            }

            interpreter.getNormalizedIndexes();
            return interpreter;
        }
    }
}
