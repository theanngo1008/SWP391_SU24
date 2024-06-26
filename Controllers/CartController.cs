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
    {
        private readonly JewelrySystemDbContext _context;
        private readonly SpotMetalPriceService _smpservice;

        public CartController(JewelrySystemDbContext context, SpotMetalPriceService smpservice)
        {
            _context = context;
            _smpservice = smpservice;
        }

        [HttpPost("AddToCart")]
        public async Task<IActionResult> AddToCart(AddToCartRequest request)
        {
            var jewelry = await _context.Jewelries.FindAsync(request.ProductId);
            if (jewelry == null)
            {
                return NotFound("Product not found!!!");
            }

    

            decimal goldPrice;
            try
            {
                goldPrice = await _smpservice.GetGoldPriceAtTime();
            }
            catch
            {
                return StatusCode(500, "Error retrieving gold price!!!");
            }

            var makingCharges = await _context.JewelryMakingCharges.FirstOrDefaultAsync(jmc => jmc.ChargeId == jewelry.ChargeId);
            if (makingCharges == null)
            {
                return BadRequest("Making Charges are not available for this jewelry!!!");
            }

            decimal gemstoneCost = 0;
            foreach (var selectedGemstone in request.Gemstones) 
            {
                var gemstone = await _context.Gemstones.FindAsync(selectedGemstone.GemstoneId);
                if (gemstone == null)
                {
                    return BadRequest(($"Gemstone with ID {selectedGemstone.GemstoneId} not found!!!"));
                }
                gemstoneCost += gemstone.GemstoneCost.Value * selectedGemstone.Quantity;
            }

            var jewelryCost = (goldPrice * request.Weight) + makingCharges.Price + gemstoneCost;

            var cart = HttpContext.Session.GetSessionObject<Cart>("Cart") ?? new Cart();

            var cartItem = cart.Items.FirstOrDefault(item => item.ProductId == request.ProductId);

            if (cartItem != null)
            {
                cartItem.Quantity += request.Quantity;
                cartItem.Weight = request.Weight;
                cartItem.Gemstones = request.Gemstones;
            }
            else
            {
                cart.Items.Add(new CartItem
                {
                    ProductId = request.ProductId,
                    Quantity = request.Quantity,
                    Price = jewelryCost.Value, //lưu giá sản phẩm vào giỏ
                    Weight = request.Weight,
                    Gemstones = request.Gemstones
                });
            }

            HttpContext.Session.SetSessionObject("Cart", cart);

            return Ok("Product added to cart");
        }

        [HttpGet("GetCart")]
        public IActionResult GetCart()
        {
            var cart = HttpContext.Session.GetSessionObject<Cart>("Cart") ?? new Cart();
            return Ok(cart);
        }

        [HttpPost("PlaceOrder")]
        public async Task<IActionResult> PlaceOrder([FromBody] int accountId)
        {
            var cart = HttpContext.Session.GetSessionObject<Cart>("Cart");
            if (cart == null || !cart.Items.Any())
            {
                return BadRequest("Cart is empty");
            }

            var order = new Order
            {
                AccId = accountId,
                OrderDate = DateOnly.FromDateTime(DateTime.Now),
                OrderDetails = new List<OrderDetail>()
            };

            foreach (var cartItem in cart.Items)
            {
                var jewelry = await _context.Jewelries.FindAsync(cartItem.ProductId);
                if (jewelry == null)
                {
                    return BadRequest($"Jewelry with ID {cartItem.ProductId} not found");
                }

                order.OrderDetails.Add(new OrderDetail
                {
                    JewelryId = cartItem.ProductId,
                    Quantity = cartItem.Quantity,
                    Price = jewelry.Cost.Value
                }); ;


            }

            _context.Orders.Add(order);
            HttpContext.Session.Remove("Cart");

            return Ok("Order placed successfully");
        }
    }
}
