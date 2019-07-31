using Jint;
using Microsoft.AspNetCore.NodeServices;
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
    public class JSScriptEngine : IScriptEngine
    {
        public async Task<object> Execute(ScriptingInputDto input)
        {
            Engine engine = new Engine()
            .SetValue("log", new Action<object>(Console.WriteLine))
            ;

            object result = null;
            try
            {
                result = engine.Execute(input.Code).GetCompletionValue().ToObject();
            }
            catch (Exception e)
            {
                List<string> templog = new List<string>();
                templog.Add(e.Message);
                return new { log = templog };
            }

            return new
            {
                result = result,
            };
        }
    }
}
