using BlogAspNet_Env.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace BlogAspNet_Env.Controllers
{
    [ApiController]
    [Route("")]
    public class HomeController : ControllerBase
    {
        [HttpGet("")]
        public IActionResult Get(
            [FromServices] IConfiguration config)
        {
            var env = config.GetValue<string>("Environment");
            return Ok(new
            {
                environment = env
            });
        }
    }
}
