namespace asp_dot_net_core_web_app_mvc_fast_food_system.Models.Base
{
    public class CustomerRemarks
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid CustomerId { get; set; }

        public Customer? Customer { get; set; }

        public string CustomerPhoneNumber { get; set; } = null!;

        public string Remark { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime LastUpdatedAt { get; set; } = DateTime.UtcNow;

        public CustomerRemarks() { }
        public CustomerRemarks
        (
            Guid customerId,
            Customer? customer,
            string customerPhoneNumber,
            string remark
        )
        {
            CustomerId = customerId;
            Customer = customer;
            CustomerPhoneNumber = customerPhoneNumber;
            Remark = remark;
        }
    }
}
