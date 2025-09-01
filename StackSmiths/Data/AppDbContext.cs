using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StackSmiths.Models;

namespace StackSmiths.Data
{
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Products> Products { get; set; } = null!;
        public DbSet<Carts>? Carts { get; set; }
        public DbSet<Wishlists>? Wishlists { get; set; }
        public DbSet<Orders>? Orders { get; set; }
        public DbSet<OrderItems>? OrderItems { get; set; }
        public DbSet<CartItems>? CartItems { get; set; }
        public DbSet<WishlistItem>? WishlistItems { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Products>()
                .Property(p => p.Price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Orders>()
                .Property(o => o.TotalAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<OrderItems>()
                .Property(oi => oi.Price)
                .HasPrecision(18, 2);
        }

    }

}
