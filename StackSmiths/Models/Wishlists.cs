namespace StackSmiths.Models
{
    public class Wishlists
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;


        public int ProductId { get; set; }
        public Products Products { get; set; } = null!;
    }
}
