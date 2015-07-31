using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Algoritm_library;
using TestApp1.Result;
using System.Reflection;

namespace TestApp1.Algorithm
{
    abstract class CompareAlgorithm
    {
        
        public abstract ResultInterpreterOpt check(ResultInterpreterOpt interpreter, String dbText, String userText);
        protected algorithms patternAlgorithms = new algorithms();
        public String patternAlgorithmName {get; set;}
        private MethodInfo patternAlgorithm = null;

        protected String findPattern(String dbText, String pat)
        {
            this.patternAlgorithm = patternAlgorithms.GetType().GetMethod(this.patternAlgorithmName);
            String[] args = new String[2];
            args[0] = dbText;
            args[1] = pat;
            if (dbText == "")
            {
                return "";
            }
            else 
            {
                return (String)patternAlgorithm.Invoke(this.patternAlgorithms, args);
            }
            
        }

    }
}
