using Microsoft.AspNetCore.Identity;

namespace BE.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string AccName { get; set; } = null;
        public decimal? Deposit {  get; set; }
        public string? Address { get; set; }
        public int? Status {  get; set; }
        public virtual LoyaltyCard? LoyaltyCard { get; set; }
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
        public virtual ICollection<Quotation> Quotations { get; set; } = new List<Quotation>();
    }
}
