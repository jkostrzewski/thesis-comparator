using RazorEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TestApp1.Result;
using RazorEngine.Templating;


namespace TestApp1
{
    class ReportRenderer
    {
        public static String Generate(ResultInterpreter interpreter, String dbText, String userText)
        {
            String template = new StreamReader("../../Report/ReportTemplate.html").ReadToEnd();
            var model = new { I = interpreter, dbText = dbText, userText = userText };
            String result = RazorEngine.Engine.Razor.RunCompile(template, "resultKeyUnique", null, model);
            return result;
        }
    }
}
