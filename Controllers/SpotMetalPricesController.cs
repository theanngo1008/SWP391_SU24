﻿using BE.Entities;
using BE.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpotMetalPricesController : ControllerBase
    {
        private readonly SpotMetalPriceService _service;

        public SpotMetalPricesController (SpotMetalPriceService service)
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
        public async Task<ActionResult<IEnumerable<SpotMetalPrice>>> GetGoldPricesForYear2024()
        {
            var prices = await _service.GetGoldPricesForYear2024();
            return Ok(prices);
        }


    }
}
