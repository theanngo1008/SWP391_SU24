using BE.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization.Metadata;

namespace BE.Services
{
    public class QuotationService
    { /*
        private readonly JewelrySystemDbContext _context;
        private readonly OrderService _service;

        public QuotationService(JewelrySystemDbContext context)
        {
            _context = context;
        }
        
        public async Task<ActionResult> CreateQuotationFromOrđer(int orderId)
        {

            var account = await _context.Orders.FindAsync(order.AccId);
            if (account == null)
            {
                throw new KeyNotFoundException("Account not found!!!");
            }

            var orderDetails = await _context.OrderDetails
                                .Where(od => od.OrderId == order.OrderId)
                                .ToListAsync();

            if (orderDetails != null)
            {
                throw new InvalidOperationException("Order has no details");
            }

            var quotation = new Quotation
            {
                //OrderId = orderId,
                //AccId = order.AccId,
            };
        } */
    }
}
