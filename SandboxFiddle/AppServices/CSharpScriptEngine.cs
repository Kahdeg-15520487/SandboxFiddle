using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using SandboxFiddle.Contract;
using SandboxFiddle.Contract.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandboxFiddle.AppService
{
    public class CSharpScriptEngine : IScriptEngine
    {
        public class Globals
        {
            public Dictionary<string, string> Database;
            public List<object> logger;
        }

        public async Task<object> Execute(ScriptingInputDto input)
        {
            string code = input.Code;
            string[] imports = input.Imports;
            ScriptState<object> scriptState = null;

            code = code.Replace("this.", "");
            if (code.Contains("Console.WriteLine"))
            {
                code = code.Replace("Console.WriteLine", "logger.Add");
            }

            List<string> importList = imports == null ? new List<string>() : imports.ToList();
            importList.Add("System");
            importList.Add("System.Linq");
            importList.Add("System.Collections.Generic");

            ScriptOptions opts = ScriptOptions.Default
                .AddImports(importList)
                .AddReferences(typeof(System.Linq.Enumerable).Assembly.Location)
                .AddReferences(typeof(System.Linq.Expressions.BinaryExpression).Assembly.Location)
                .AddReferences(typeof(System.Collections.Generic.AsyncEnumerator).Assembly.Location)
                ;
            List<object> logger = new List<object>();
            string[] log = null;
            Globals globals = new Globals
            {
                logger = logger
            };

            try
            {
                scriptState = await CSharpScript.RunAsync(code, opts, globals);
                log = logger.Select(l => l.ToString()).ToArray();
            }
            catch (Exception e)
            {
                List<string> templog = log == null ? new List<string>() : log.ToList();
                templog.Add(e.Message);
                return new { log = templog };
            }

            if (scriptState.ReturnValue != null && !string.IsNullOrEmpty(scriptState.ReturnValue.ToString()))
            {
                return new
                {
                    result = scriptState.ReturnValue,
                    log = log
                };
            }
            else
            {
                return new
                {
                    log = log
                };
            }
        }
    }
}
