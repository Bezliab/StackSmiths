namespace StackSmiths.Models
{
    public class Wishlist
    {
        public int Id { get; set; }
        public string UserId { get; set; }

        // Foreign Key to Product
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
