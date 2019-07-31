using Microsoft.Extensions.Logging;
using SandboxFiddle.Contract;
using SandboxFiddle.Contract.Dtos;

namespace SandboxFiddle.AppService
{
    class ScriptingService : IScriptingService
    {
        private ILogger logger;
        private IScriptEngine scriptEngine;

        public ScriptingService(IScriptEngine scriptEngine, ILoggerFactory loggerFactory)
        {
            this.scriptEngine = scriptEngine;
            this.logger = loggerFactory.CreateLogger<ScriptingService>();
        }


        public object Execute(ScriptingInputDto script)
        {
            return scriptEngine.Execute(script);
        }
    }
}
