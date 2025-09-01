namespace StackSmiths.Models
{
    public class CartItems
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;

        public int ProductId { get; set; }
        public Products Products { get; set; } = null!;

        public int Quantity { get; set; }
    }
}
