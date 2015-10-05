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
            
            DbInterface i = new DbInterface("localhost");
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
            //var userId = "0372e002-5bf3-445d-a851-f85ff752b5bf";
            //var userId = "63a169a4-f05b-4072-b747-83651eb0a0fd";
            

            var path = "";
            if (args.Length >3 )
            {
                path = args[3];
            }
            //String userText = Utils.normalizeText(Utils.getDocumentFromFile(path));
            String userText = Utils.normalizeDbText(i.GetDocument(path));
            Console.Out.WriteLine(userText);
            //Console.ReadKey();
            //String userText = i.GetDocument(userId);
            var jsonReport = new JsonReportBuilder(Path.GetFileName(path), userText, compareAlgName, patternAlgName, noThreads);
            ParallelOptions parallelOptions = new ParallelOptions();
            parallelOptions.MaxDegreeOfParallelism = noThreads;
            DateTime startTime = DateTime.Now;
            int progress = 0;
            Object progressLock = new Object();
            Parallel.ForEach (i.GetDocumentIds(), parallelOptions, id  =>{
                if (path == id)
                {
                    return;
                }
                lock (progressLock) { progress++; }
                Console.Out.Write(progress + " ");
                String dbText = Utils.normalizeDbText(i.GetDocument(id));
               // Console.Out.WriteLine(dbText);
                //Console.Out.WriteLine(userText);
                
                //Console.ReadKey();
                String name = i.GetDocumentName(id);
                ResultInterpreterOpt interpreter = new ResultInterpreterOpt(dbText, userText);
                interpreter.isBenchmark = false;
                Console.Out.WriteLine("Checking");
                interpreter = compareAlgorithm.check(interpreter, dbText, userText);
                Console.Out.WriteLine("Updating report");
                jsonReport.AddInterpreter(name, id, interpreter);
                Console.Out.WriteLine("Report updated");
                
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
