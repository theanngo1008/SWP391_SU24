using BE.Entities;

namespace BE.Extensions
{
    public class JewelryGemstone
    { 
        public int? JewelryId { get; set; }

        public int? GemstoneId { get; set; }

        public virtual Jewelry? Jewelry { get; set; }

        public virtual Gemstone? Gemstone { get; set; } 
    }
}  
