using System.ComponentModel.DataAnnotations;

namespace asp_dot_net_core_web_app_mvc_fast_food_system.Models.Base
{
    public class Customer
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string? Name { get; set; } = null!;

        [Required]
        [RegularExpression(@"^\(?\d{3}\)?[- ]?\d{3}[- ]?\d{4}$", ErrorMessage = "Phone number must be in format (123) 456-7890 or 123-456-7890")]
        public string PhoneNumber { get; set; } = null!;
        public string? Address { get; set; } = null!;

        public List<Dictionary<DateTime, string>>? Observations { get; set; } = new List<Dictionary<DateTime, string>>();

        public HashSet<Order>? Orders { get; set; } = new HashSet<Order>();

        public Customer() { }

        public Customer
        (
            string? name,
            string phoneNumber,
            string? Address,
            List<Dictionary<DateTime, string>>? observations,
            HashSet<Order>? orders
        )
        {
            Name = name;
            PhoneNumber = phoneNumber;
            Address = Address;
            Observations = observations;
            Orders = orders;
        }
    }
}
