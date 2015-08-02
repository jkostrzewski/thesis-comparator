using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TestApp1.Algorithm;
using TestApp1.Report;
using TestApp1.Result;
using TestApp1.Statistics;
using System.Diagnostics;



namespace TestApp1
{
    class Program
    {
    
        static void Main(string[] args)
        {
            String patternAlgName = "KMP";
            String compareAlgName = "SentenceSplit";
            int noThreads = 1;
            
            DbInterface i = new DbInterface("192.168.9.103");
            CompareAlgorithm compareAlgorithm;
            if (args.Length > 0)
            {
                Console.Out.WriteLine(args[0] + " " + args[1]);
                compareAlgName = args[0];
                patternAlgName = args[1];
                noThreads = Int32.Parse(args[2]);
                var assembly = Assembly.GetExecutingAssembly().GetTypes();
                compareAlgorithm = (CompareAlgorithm)Activator.CreateInstance(Type.GetType("TestApp1.Algorithm." + compareAlgName));
                compareAlgorithm.patternAlgorithmName = patternAlgName;
              
            }
            else
            {
                compareAlgorithm = new SentenceSplit();
            }
            var userId = "9dabbef3-8bcf-4b63-8424-d3ed1aa8392a";
            var path = args[3];
            //String userText = Utils.normalizeText(Utils.getDocumentFromFile(path));
            String userText = Utils.normalizeText(i.GetDocument(userId));
            var jsonReport = new JsonReportBuilder(Path.GetFileName(path), userText, compareAlgName, patternAlgName, noThreads);
            ParallelOptions parallelOptions = new ParallelOptions();
            parallelOptions.MaxDegreeOfParallelism = noThreads;
            DateTime startTime = DateTime.Now;
            Parallel.ForEach (i.GetDocumentIds(), parallelOptions, id  =>{
                if (userId == id)
                {
                    return;
                }
                String dbText = Utils.normalizeText(i.GetDocument(id));
                String name = i.GetDocumentName(id);
                ResultInterpreterOpt interpreter = new ResultInterpreterOpt(dbText, userText);
             
                interpreter = compareAlgorithm.check(interpreter, dbText, userText);
               
                jsonReport.AddInterpreter(name, id, interpreter);
                
            });
            foreach (String id in i.GetDocumentIds())
            {
                jsonReport.setIdMapping(id, i.GetDocumentName(id));
            }
            DateTime endTime = DateTime.Now;
            jsonReport.setTime((float)endTime.Subtract(startTime).TotalMilliseconds/1000);
            jsonReport.generate();

            Console.Out.WriteLine("Koniec");
            //Console.ReadKey();

        }
    }
}
