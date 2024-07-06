using JewelryProductionOrder.BusinessLogic.Services.Implementation;
//using JewelryProductionOrder.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace JewelryProductionOrder.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpotMetalPricesController : ControllerBase
    {  /*
        private readonly SpotGoldPriceService _service;

        public SpotMetalPricesController(SpotGoldPriceService service)
        {
            _service = service;
        }

        [HttpGet("UpdateGoldPrices")]
        public async Task<IActionResult> UpdateGoldPrices()
        {
            await _service.UpdateSpotMetalPrice();
            return Ok("Gold prices updated successfully");
        }

        [HttpGet("GetGoldPricesForYear2024")]
        public async Task<ActionResult<IEnumerable<SpotGoldPrice>>> GetGoldPricesForYear2024()
        {
            var prices = await _service.GetGoldPricesForYear2024();
            return Ok(prices);
        }    */
    }
}
