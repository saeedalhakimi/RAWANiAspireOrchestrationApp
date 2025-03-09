using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RAWANiAspireOrchestrationApp.ApiService.Presentation.Controllers.V2
{
    [Route(ApiRoutes.BaseRoute)] // api/v2/VersionTest
    [ApiVersion("2")]
    [ApiController]
    public class VersionTestController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Version 2");
        }
    }
}
