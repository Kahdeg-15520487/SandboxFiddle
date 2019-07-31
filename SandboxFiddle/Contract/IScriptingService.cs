using SandboxFiddle.Contract.Dtos;

namespace SandboxFiddle.Contract
{
    public interface IScriptingService
    {
        object Execute(ScriptingInputDto script);
    }
}
