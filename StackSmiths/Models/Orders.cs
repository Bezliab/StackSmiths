namespace StackSmiths.Models
{
    public class Orders
    {
        public int Id { get; set; }

        public string UserId { get; set; } = string.Empty;

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        public decimal TotalAmount { get; set; }

        public ICollection<OrderItems> Items { get; set; } = new List<OrderItems>();
    }
}
