using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp1.Result
{
    class ResultInterpreter
    {
        public List<ResultIndex> indexes {get;set;}
        private String dbText, userText;

        public ResultInterpreter(String dbText, String userText)
        {
            this.indexes = new List<ResultIndex>();
            this.dbText = dbText;
            this.userText = userText;
        }

        public void addIndex(ResultIndex i)
        {
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
            this.indexes = this.indexes.OrderBy(o => o.id).ToList<ResultIndex>();
            foreach (ResultIndex r1 in this.indexes)
            {
                if (r1.active == false)
                {
                   // Console.Out.WriteLine("skip");
                    continue;
                }
                r1.markAsNotActive();
                ResultIndex index = new ResultIndex(r1.id, r1.userStart, r1.userEnd, r1.dbStart, r1.dbEnd);
                foreach (ResultIndex r2 in this.indexes)
                {
                    if (r1.id == r2.id || r2.active == false)
                    {
                        //Console.Out.WriteLine("skip");
                        continue;
                    }
                    if (index.userEnd+1  >= r2.userStart &&
                        index.userStart < r2.userStart &&
                        index.dbEnd+1  >= r2.dbStart &&
                        index.dbStart < r2.dbStart)
                    {
                        index.userEnd = Math.Max(r2.userEnd, index.userEnd);
                        index.dbEnd = Math.Max(r2.dbEnd, index.dbEnd);
                        r1.userEnd = Math.Max(r2.userEnd, r1.userEnd);
                        r1.dbEnd = Math.Max(r2.dbEnd, r1.dbEnd);

                        r2.markAsNotActive();

                    }
                }
                
                //Console.Out.WriteLine(index.ToString());
                //Console.Out.WriteLine(userText.Substring(index.userStart, index.userEnd - index.userStart));
                //Console.Out.WriteLine(dbText.Substring(index.dbStart, index.dbEnd - index.dbStart));
                //Console.Out.WriteLine();
                result.Add(index);
            }
            this.indexes = result;
        }


    }



}
