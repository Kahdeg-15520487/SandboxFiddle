using SandboxFiddle.Contract.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SandboxFiddle.Contract
{
    public enum SupportedScriptEngine
    {
        Js,
        Csharp
    }
    public interface IScriptEngine
    {
        Task<object> Execute(ScriptingInputDto input);
    }
}
