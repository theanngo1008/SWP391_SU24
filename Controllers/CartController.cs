using BE.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BE.Extensions;
using BE.Entities;


namespace BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly JewelrySystemDbContext _context;

        public CartController(JewelrySystemDbContext context)
        {
            _context = context;
        }

        [HttpPost("AddToCart")]
        public async Task<IActionResult> AddToCart(int productId, int quantity)
        {
            var jewelry = await _context.Jewelries.FindAsync(productId);
            if (jewelry == null)
            {
                return NotFound("Product not found!!!");
            }

            var cart = HttpContext.Session.GetSessionObject<Cart>("Cart") ?? new Cart();

            var cartItem = cart.Items.FirstOrDefault(item => item.ProductId == productId);

            if (cartItem != null)
            {
                cartItem.Quantity += quantity;
            }
            else
            {
                cart.Items.Add(new CartItem
                {
                    ProductId = productId,
                    Quantity = quantity,
                    Price = jewelry.Cost.Value //lưu giá sản phẩm vào giỏ
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
            await _context.SaveChangesAsync();
            HttpContext.Session.Remove("Cart");

            return Ok("Order placed successfully");
        }
    }
}
