﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestApp1.Result;

namespace TestApp1.Algorithm
{
    class WordSplit : CompareAlgorithm
    {
        
        public override ResultInterpreterOpt check(ResultInterpreterOpt interpreter, String dbText, String userText)
        {
            int sliceIndex = 0;
            int resultIndexId = 0;
            foreach (String slice in Splitter.SplitByNGram(userText, 1))
            {        
                if (slice == "")
                {
                    sliceIndex++;
                    continue;
                }
                //Console.Out.WriteLine(slice);
                String r = findPattern(dbText, " "+slice+" ");
                var indexes = r.Split(';').Where(x => x != "").Select(x => Convert.ToInt32(x)).ToList();
                //Console.Out.WriteLine(indexes.Count)
                foreach (int i in indexes)
                {
                    interpreter.addIndex(resultIndexId, sliceIndex+1, sliceIndex + slice.Length+1, i+1, i+slice.Length+1);
                    resultIndexId++;
                    // Console.Out.WriteLine();
                    //Console.Out.WriteLine(slice);
                }
                sliceIndex = sliceIndex + slice.Length+1;
            }

            interpreter.getNormalizedIndexes();
            return interpreter;
        }
    }
}
