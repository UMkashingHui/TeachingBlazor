using Microsoft.AspNetCore.Mvc;

namespace TeachingWebApi.Controllers;

[ApiController]
[Route("api/")]
public class HealthCheckController : ControllerBase
{

    [HttpGet("healthcheck")]
    public String Get()
    {
        return "the@health@is@good";
    }
}

