namespace StackSmiths.Models
{
    public class Carts
    {
        public int Id { get; set; }


        public string UserId { get; set; } = string.Empty;
        public int ProductId { get; set; }
        public Products Product { get; set; } = null!;

        public int Quantity { get; set; }
    }
}
