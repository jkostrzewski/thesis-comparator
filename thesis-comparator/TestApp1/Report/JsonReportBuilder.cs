using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using TestApp1.Result;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using Newtonsoft.Json;

namespace TestApp1.Report
{
    class JsonReportBuilder
    {
        JObject root = new JObject();
        
        Object addJsonResultLock = new Object();
        static String timestamp = null;
        static String path = @"C:\Users\kuba\Desktop\thesis-comparator\report-renderer\";
        String jsonFilePath;
       

        public JsonReportBuilder(String userTextId, String userText, String compareName, String patternName, int noThreads)
        {
            root["userText"] = new JObject();
            root["meta"] = new JObject();
            root["meta"]["compareAlgorithmName"] = compareName;
            root["meta"]["patternAlgorithmName"] = patternName;
            root["meta"]["noThreads"] = noThreads;
            root["userText"]["id"] = userTextId;
            root["userText"]["text"] = userText;
            root["matched"] = new JArray();
            root["id_mapping"] = new JObject();

            timestamp = DateTime.Now.ToString("yyMMdd_HHmmss");
            jsonFilePath = path + @"comparator-data\data_" + timestamp + ".json";
            System.IO.StreamWriter file = new System.IO.StreamWriter(jsonFilePath);
            file.WriteLine(root.ToString());
            file.Close();
        }

        public void AddInterpreter(String name, String docId, ResultInterpreterOpt interpreter)
        {
            List<JsonMatchedModel> matched = new List<JsonMatchedModel>();
            foreach (ResultIndex r in interpreter.indexes){
                JsonMatchedModel d = new JsonMatchedModel();
                d.id = docId;
                d.us = r.userStart;
                d.ue = r.userEnd;
                d.ds = r.dbStart;
                d.de = r.dbEnd;
                matched.Add(d);
            }

            lock (addJsonResultLock)
            {
                JsonDataModel m = JsonConvert.DeserializeObject<JsonDataModel>(System.IO.File.ReadAllText(jsonFilePath));
                m.matched.AddRange(matched);
                System.IO.File.WriteAllText(jsonFilePath, JsonConvert.SerializeObject(m));
            }
            interpreter = null;
        }
        public void setTime(float time)
        {
            JsonDataModel m = JsonConvert.DeserializeObject<JsonDataModel>(System.IO.File.ReadAllText(jsonFilePath));
            m.meta["time"] = time.ToString();
            System.IO.File.WriteAllText(jsonFilePath, JsonConvert.SerializeObject(m));
        }

        public String read()
        {
            System.IO.StreamWriter file = new System.IO.StreamWriter("result.json");
            //Console.Out.WriteLine(root.ToString());
            file.WriteLine(root.ToString());
            return root.ToString();
        }

        public void generate()
        {   
            var outputFilePath = path + @"reports\report"+ timestamp + ".html";
            var pythonFilePath = path + @"renderer.py";
            
            ScriptRuntime python = Python.CreateRuntime();
            dynamic pyfile = python.UseFile(pythonFilePath);
            pyfile.main(jsonFilePath, outputFilePath);
           
        }

        internal void setTime(TimeSpan timeSpan)
        {
            throw new NotImplementedException();
        }
        public void setIdMapping(String id, String name)
        {
            JsonDataModel m = JsonConvert.DeserializeObject<JsonDataModel>(System.IO.File.ReadAllText(jsonFilePath));
            m.id_mapping[id] = name;
            System.IO.File.WriteAllText(jsonFilePath, JsonConvert.SerializeObject(m));
        }
    }

    class JsonDataModel
    {
        public Dictionary<String, String> userText;
        public Dictionary<String, String> meta;
        public Dictionary<String, String> id_mapping;
        public List<JsonMatchedModel> matched;
    }
    class JsonMatchedModel
    {
        public String id;
        public int us;
        public int ue;
        public int ds;
        public int de;
    }
}

