namespace StackSmiths.Models
{
    public class Cart
    {
        public int Id { get; set; }

        // Relationship to User (simplified as string for now, later you can link to a User entity)
        public string UserId { get; set; }

        // Foreign Key to Product
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int Quantity { get; set; }
    }
}
