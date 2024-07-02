using BE.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BE.Extensions;
using BE.Entities;
using Microsoft.EntityFrameworkCore;
using BE.Services;


namespace BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {  /*
        private readonly CartService _service;
        public CartController(CartService service)
        {
            _service = service;
        }

        [HttpPost("AddToCart")]
        public async Task<IActionResult> AddToCart(AddToCartRequest request)
        {
            try 
            {
                await _service.AddToCart(request);
                return Ok("Product added to cart");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidCastException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetCart")]
        public IActionResult GetCart()
        {
            var cart = _service.GetCart();
            return Ok(cart);
        }

        [HttpPost("PlaceOrder")]
        public async Task<IActionResult> PlaceOrder([FromBody] int accountId)
        {
            try
            {
                await _service.PlaceOrder();
                return Ok("Order placed successfully");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            } 
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }*/
    }
}
