using System.ComponentModel.DataAnnotations;

namespace BE.Models
{
    public class RegisterRequest
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string FullName { get; set; }

        [RegularExpression(@"^\+?[0-9]{7,15}$", ErrorMessage = "Invalid phone number format. It must be a number or start with + and contain 7 to 15 digits.")]
        public string NumberPhone { get; set; }

        public string Address { get; set; }
    }
}
