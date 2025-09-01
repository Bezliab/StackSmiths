namespace StackSmiths.Models
{
    public class WishlistItem
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;

        // Foreign key to Product
        public int ProductId { get; set; }
        public Products Product { get; set; } = null!;
    }
}
