using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp1.Result
{
    class ResultIndex
    {
        public int id {get; set;}
        public int userStart { get; set; }
        public int userEnd { get; set; }
        public int dbStart { get; set; }
        public int dbEnd { get; set; }
        public Boolean active = true;
        private int overlapFuzziness = 20; //characters
        public ResultIndex(int id, int us, int ue, int ds, int de)        
        {
            this.id = id;
            this.userStart = us;
            this.userEnd = ue;
            this.dbStart = ds;
            this.dbEnd = de;

        }

        public override string ToString()
        {
            return userStart.ToString() + " " + userEnd.ToString() + " "+ dbStart.ToString() + " "+dbEnd.ToString();
        }

        public void markAsNotActive()
        {
            this.active = false;
        }

        public int getLength()
        {
            return this.userEnd - this.userStart;
        }

        public Boolean overlap(ResultIndex r2)
        {
            
            if (this.userEnd+overlapFuzziness  >= r2.userEnd &&
                        this.userStart <= r2.userEnd + overlapFuzziness &&
                        this.dbEnd + overlapFuzziness >= r2.dbEnd &&
                        this.dbStart <= r2.dbEnd + overlapFuzziness)
            {
                return true;
            }
            else if (this.userStart <= r2.userStart + overlapFuzziness &&
                        this.userEnd + overlapFuzziness >= r2.userStart &&
                        this.dbStart <= r2.dbStart + overlapFuzziness &&
                        this.dbEnd+overlapFuzziness >= r2.dbStart)
            {
                return true;
            }
            else
            {
                return false;
            }
            
        }

    }
}
