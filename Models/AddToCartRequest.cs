//using BE.Entities;

namespace BE.Models
{
    public class AddToCartRequest
    {
        public int ProductId { get; set; }

        public int Quantity { get; set; }

        public decimal Weight { get; set; }

        public List<SelectedGemstone> Gemstones { get; set; }
    }
}
