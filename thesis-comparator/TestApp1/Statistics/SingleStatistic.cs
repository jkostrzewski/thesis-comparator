using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestApp1.Result;

namespace TestApp1.Statistics
{
    class SingleStatistic
    {
        private ResultInterpreterOpt interpreter = null;
        private String userText;
        private String dbText;

        public SingleStatistic(ResultInterpreterOpt interpreter, String userText, String dbText)
        {
            this.dbText = dbText;
            this.userText = userText;
            this.interpreter = interpreter;
        }

        public void getStatistics()
        {
            ResultIndex longestMatch = null;
            bool[] coveredUserText = new bool[this.userText.Length];
            List<ResultIndex> indexes = this.interpreter.indexes;
            if (indexes.Count == 0)
            {
                return;
            }
            foreach (ResultIndex i in indexes)
            {
                if (longestMatch == null)
                {
                    longestMatch = i;
                }
                else
                {
                    if (longestMatch.getLength() < i.getLength())
                    {
                        longestMatch = i;
                    }
                }

                for (int j = i.userStart; j < i.userEnd; j++)
                {
                    coveredUserText[j] = true;
                }
            }

            float covered = (float)coveredUserText.Where(entry => entry).Count()/userText.Length;
            Console.Out.WriteLine("longest match " + longestMatch.getLength());
            Console.Out.WriteLine("match ratio " + covered);
        }
    }
}
