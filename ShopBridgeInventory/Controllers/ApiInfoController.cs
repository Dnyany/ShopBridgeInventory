using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ShopBridgeInventory.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiInfoController : ControllerBase
    {
        private readonly ILogger<ApiInfoController> _logger;

        public ApiInfoController(ILogger<ApiInfoController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new
            {
                InventoryDetails = "ShopBridgeInventory"
            });

        }
    }
}
