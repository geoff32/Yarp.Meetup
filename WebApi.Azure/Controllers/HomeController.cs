using Microsoft.AspNetCore.Mvc;

namespace WebApi.Azure.Controllers;

[ApiController]
[Route("api/v2/home")]
public class HomeController : ControllerBase
{
    [HttpGet(Name = "GetHome")]
    public IActionResult Get()
    {
        return Ok("Azure");
    }
}
