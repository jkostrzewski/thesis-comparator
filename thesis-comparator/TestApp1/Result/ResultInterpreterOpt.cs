using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp1.Result
{
    class ResultInterpreterOpt
    {
         public List<ResultIndex> indexes {get;set;}
        private String dbText, userText;
        public int minNoWords = 3;
        public bool isBenchmark = false;

        public ResultInterpreterOpt(String dbText, String userText)
        {
            this.indexes = new List<ResultIndex>();
            this.dbText = dbText;
            this.userText = userText;
        }

        public void addIndex(ResultIndex i)
        {
            this.indexes.Add(i);
        }

        public void addIndex(int resultIndexId, int us, int ue, int ds, int de)
        {
            if (isBenchmark){
                return;
            }
            var i = new ResultIndex(resultIndexId, us, ue, ds, de);
            this.indexes.Add(i);
        }

        public void matchedToUpper()
        {

        }

        public String getNormalizedIndexes()
        {
            this.normalizeIndexes();
            return this.indexes.ToString();
        }

        public List<ResultIndex> getIndexes()
        {
            return this.indexes;
        }

        private void normalizeIndexes()
        {
            List<ResultIndex> result = new List<ResultIndex>();
            int ix = 0;
            foreach (ResultIndex r1 in this.indexes)
            {
                //ix++;
               // Console.Out.WriteLine(ix + "/" + this.indexes.Count());
                Boolean ifAddR1 = true;
                ResultIndex enhancedIndex = null;
                foreach (ResultIndex r2 in result)
                {
                    if (r1.overlap(r2))
                    {
                        int us = Math.Min(r1.userStart, r2.userStart);
                        int ue = Math.Max(r1.userEnd, r2.userEnd);
                        int ds = Math.Min(r1.dbStart, r2.dbStart);
                        int de = Math.Max(r1.dbEnd, r2.dbEnd);
                        enhancedIndex = new ResultIndex(r2.id, us, ue, ds, de);
                        ifAddR1 = false;
                        break;
                    }
                    else
                    {
                        //Console.Out.WriteLine("not overlaps");
                    }
                }
                if (ifAddR1)
                {
                    result.Insert(0, r1);
                    
                }
                else
                {
                    var i = result.FindIndex(x => x.id == enhancedIndex.id);
                    //Console.Out.WriteLine(i);
                    result[i] = enhancedIndex;
                }
            }
            //foreach (ResultIndex index in result)
            //{
             //   Console.Out.WriteLine(userText.Substring(index.userStart, index.userEnd - index.userStart));
              //  Console.Out.WriteLine(dbText.Substring(index.dbStart, index.dbEnd - index.dbStart));
               //Console.Out.WriteLine();
            //}
            this.indexes = result;
            removeShortMatches();
        }

        private void removeShortMatches()
        {
            List<ResultIndex> result = new List<ResultIndex>();
            foreach (ResultIndex index in this.indexes)
            {
                var s = userText.Substring(index.userStart, index.userEnd - index.userStart);
                if (s.Split(' ').Length >= this.minNoWords)
                {
                    result.Add(index);
                }
            }
            this.indexes = result;
        }

    }
}

