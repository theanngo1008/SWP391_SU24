using System.Xml;

namespace BE.Models
{
    public class CartItem
    {
        public int ProductId { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }  //lưu giá sản phẩm tại thời điểm thêm vào giỏ
    }

    
}
