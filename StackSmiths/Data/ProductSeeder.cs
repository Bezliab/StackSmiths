using StackSmiths.Models;

namespace StackSmiths.Data
{
    public static class ProductSeeder
    {
        public static async Task SeedProductsAsync(AppDbContext context)
        {
            if (!context.Products!.Any())
            {
                var products = new List<Products>
                {
                    new Products { Name = "Wooden Chair", Description = "Solid oak chair", Price = 59.99m, Stock = 10 },
                    new Products { Name = "Dining Table", Description = "4-seater dining table", Price = 199.99m, Stock = 5 },
                    new Products { Name = "Sofa", Description = "3-seater fabric sofa", Price = 499.99m, Stock = 2 },
                    new Products { Name = "Bookshelf", Description = "Tall wooden bookshelf", Price = 129.99m, Stock = 8 },
                    new Products { Name = "Bed Frame", Description = "King size wooden bed frame", Price = 299.99m, Stock = 3 }
                };

                context.Products!.AddRange(products);
                await context.SaveChangesAsync();
            }
        }
    }
}
