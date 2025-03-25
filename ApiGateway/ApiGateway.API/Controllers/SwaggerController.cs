// ApiGateway.API/Controllers/SwaggerController.cs
using Microsoft.AspNetCore.Mvc;

namespace ApiGateway.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SwaggerController : ControllerBase
    {
        // Example endpoint for Swagger
        [HttpGet("ping")]
        public IActionResult Ping()
        {
            return Ok("API Gateway is running!");
        }
    }
}