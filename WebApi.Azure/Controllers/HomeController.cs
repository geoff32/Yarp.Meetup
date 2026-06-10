using Microsoft.AspNetCore.Mvc;

namespace WebApi.Azure.Controllers;

[ApiController]
[Route("api/home")]
public class HomeController : ControllerBase
{
    [HttpGet(Name = "GetHome")]
    public IActionResult Get()
    {
        return Ok("Azure");
    }
}
