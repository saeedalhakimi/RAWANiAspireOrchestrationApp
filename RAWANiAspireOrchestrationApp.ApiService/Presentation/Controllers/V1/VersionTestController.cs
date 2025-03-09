using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RAWANiAspireOrchestrationApp.ApiService.Presentation.Controllers.V1
{
    [Route(ApiRoutes.BaseRoute)] // api/v1/VersionTest
    [ApiVersion("1")]
    [ApiController]
    public class VersionTestController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Version 1");
        }
    }
}
