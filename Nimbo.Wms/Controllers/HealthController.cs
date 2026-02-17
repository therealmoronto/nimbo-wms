using Microsoft.AspNetCore.Mvc;

namespace Nimbo.Wms.Controllers;

[ApiController]
[Route("api/health")]
public sealed class HealthController : ControllerBase
{
    /// <summary>
    /// Health check endpoint
    /// </summary>
    [HttpGet]
    public IActionResult Get() => Ok(new { status = "ok" });
}
