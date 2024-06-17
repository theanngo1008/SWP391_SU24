using BE.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly JewelrySystemDbContext _context;

        public OrderController(JewelrySystemDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("CreateOrder")]
        public async Task<IActionResult> CreateOrder([FromBody] OrderRequest orderRequest)
        {
            if (orderRequest == null)
            {
                return BadRequest("Invalid order data!!!");
            }

            var accId = User.Claims.FirstOrDefault(a => a.Type == "AccId")?.Value;
            if (accId == null)
            {
                return BadRequest("User not found!!!");
            }

            var account = await _context.Accounts.FindAsync(int.Parse(accId));

            if (account == null)
            {
                return BadRequest("Account not found!!!");
            }

            var shipping = await _context.Shippings.SingleOrDefaultAsync(s => s.ShippingName == orderRequest.ShippingName);
            if (shipping == null)
            {
                return BadRequest("Shipping method not found!!!");
            }
            var order = new Order
            {
                AccId = account.AccId,
                ShippingId = shipping.ShippingId,
                OrderDate = DateOnly.FromDateTime(DateTime.UtcNow),
                OrderStatus = orderRequest.OrderStatus,
                Status = orderRequest.Status,
                OrderDetails = orderRequest.OrderDetails.Select(od => new OrderDetail
                {
                    Quantity = od.Quantity,
                    CreateDate = DateOnly.FromDateTime(DateTime.UtcNow),
                    DetailStatus = od.DetailStatus,
                    JewelryId = od.JewelryId
                }).ToList()
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            var response = new
            {
                order.OrderId,
                order.OrderDate,
                order.OrderStatus,
                order.Status,
                account.AccName,
                ShippingName = shipping?.ShippingName,
                OrderDetail = order.OrderDetails.Select(od => new
                {
                    od.OrderDetailId,
                    od.Quantity,
                    od.CreateDate,
                    od.DetailStatus,
                    od.JewelryId,
                    od.OrderId
                })
            };
            return Ok(response);
        }
    }

    public class OrderRequest
    {
        public string AccName { get; set; }
        public string ShippingName { get; set; }
        public DateOnly OrderDate { get; set; }
        public string OrderStatus { get; set; }
        public bool Status { get; set; }
        public List<OrderDetailRequest> OrderDetails { get; set; }
    }

    public class OrderDetailRequest
    {
        public int Quantity { get; set; }
        public string DetailStatus { get; set; }
        public int JewelryId { get; set; }
    }
}
