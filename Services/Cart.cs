using BE.Models;

namespace BE.Services
{
    public class Cart
    {
        public List<CartItem> Items { get; set; } = new List<CartItem>();
    }
}
