using BE.Entities;
using BE.Extensions;
using BE.Models;
using Microsoft.EntityFrameworkCore;

namespace BE.Services
{
    public class CartService
    {  /*
        private readonly JewelrySystemDbContext _context;
        private readonly SpotGoldPriceService _smpservice;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartService(JewelrySystemDbContext context, SpotGoldPriceService smpService, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _smpservice = smpService;
            _httpContextAccessor = contextAccessor;
        }

        public async Task AddToCart(AddToCartRequest request)
        {
            var jewelry = await _context.Jewelries.FindAsync(request.ProductId);
            if (jewelry == null)
            {
                throw new KeyNotFoundException("Product not found!!!");
            }

            decimal goldPrice = await _smpservice.GetGoldPriceAtTime();
            
            var makingCharges = await _context.Fees.FirstOrDefaultAsync(jmc => jmc.FeeId ==);
            if (makingCharges == null)
            {
                throw new KeyNotFoundException("Making Charges are not available for this jewelry!!!");
            }

            decimal gemstoneCost = 0;
            foreach (var selectedGemstone in request.Gemstones)
            {
                var gemstone = await _context.Gemstones.FindAsync(selectedGemstone.GemstoneId);
                if (gemstone == null)
                {
                    throw new KeyNotFoundException(($"Gemstone with ID {selectedGemstone.GemstoneId} not found!!!"));
                }
                gemstoneCost += gemstone.GemstoneCost.Value * selectedGemstone.Quantity;
            }

            var jewelryCost = (goldPrice * request.Weight) + makingCharges.Price + gemstoneCost;

            var session = _httpContextAccessor.HttpContext.Session;
            var cart = session.GetSessionObject<Cart>("Cart") ?? new Cart();

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
            session.SetSessionObject("Cart", cart);
        }

        public Cart GetCart()
        {
            var session = _httpContextAccessor.HttpContext.Session;
            return session.GetSessionObject<Cart>("Cart") ?? new Cart();
        }

        public async Task PlaceOrder()
        {
            var session = _httpContextAccessor.HttpContext.Session;

            var cart = session.GetSessionObject<Cart>("Cart");
            if(cart == null || !cart.Items.Any())
            {
                throw new InvalidOperationException("Cart is empty");
            }

            var accountId = session.GetInt32("AccId");
            if (accountId == null)
            {
                throw new InvalidOperationException("User is not logged in!!!");
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
                    throw new KeyNotFoundException($"Jewelry with ID {cartItem.ProductId} not found!!!");
                }

                order.OrderDetails.Add(new OrderDetail
                {
                    JewelryId = cartItem.ProductId,
                    Quantity = cartItem.Quantity,
                    //price = cartItem.Price,
                });
            }

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            session.Remove("Cart");
        } */
    }
}
