using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using SandboxFiddle.Contract;
using SandboxFiddle.Contract.Dtos;
using System.Linq;
using System.Threading.Tasks;

namespace TestLinq.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScriptingController : ControllerBase
    {
        private readonly Func<SupportedScriptEngine, IScriptEngine> scriptEngines;

        public ScriptingController(Func<SupportedScriptEngine, IScriptEngine> scriptEngines)
        {
            this.scriptEngines = scriptEngines;
        }

        [HttpGet("supported")]
        public IActionResult GetSupportedLanguage()
        {
            return this.Ok(Enum.GetValues(typeof(SupportedScriptEngine)).Cast<SupportedScriptEngine>().Select(e => e.ToString()));
        }

        [HttpPost]
        public async Task<IActionResult> Execute([FromBody] ScriptingInputDto script)
        {
            return this.Ok(await this.scriptEngines(script.ScriptType).Execute(script));
        }
    }
}
