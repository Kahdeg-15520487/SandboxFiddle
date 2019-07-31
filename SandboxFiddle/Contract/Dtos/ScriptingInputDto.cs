using System;
using System.Collections.Generic;
using System.Text;

namespace SandboxFiddle.Contract.Dtos
{
    public class ScriptingInputDto
    {
        public SupportedScriptEngine ScriptType { get; set; }
        public string Code { get; set; }
        public string[] Imports { get; set; }
    }
}
