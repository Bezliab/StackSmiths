namespace StackSmiths.Models
{
    public class Order
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        public decimal TotalAmount { get; set; }

        // Each order can have multiple items
        public ICollection<OrderItem> Items { get; set; }
    }
}
