using Ecommerce.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Data
{
    public class EcommerceContext : IdentityDbContext<AspNetUser>
    {
        public EcommerceContext(DbContextOptions<EcommerceContext> options)
            : base(options)
        {
        }

        // ─── Domain DbSets ────────────────────────────────────────────────────────
        // Add your entity DbSets here as you create them, e.g.:
        public DbSet<AspNetUser> Users { get; set; } = null!;
        public DbSet<Product>   Products   { get; set; } = null!;
         public DbSet<Category>  Categories { get; set; } = null!;
         public DbSet<Orders>    Orders     { get; set; } = null!;
         public DbSet<Carts>     Carts      { get; set; } = null!;
         public DbSet<CartItem>  CartItems  { get; set; } = null!;
         public DbSet<Address>   Addresses  { get; set; } = null!;
         public DbSet<Payments>  Payments   { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // ── Identity table rename (optional but common for clean schema) ──────
            builder.Entity<AspNetUser>(b =>
            {
                b.Property(u => u.FirstName).HasMaxLength(100).IsRequired();
                b.Property(u => u.MiddleName).HasMaxLength(100);
                b.Property(u => u.LastName).HasMaxLength(100).IsRequired();
            });

            builder.Entity<IdentityRole>().ToTable("Roles");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
            builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
            builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
            builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");

            builder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId);

            builder.Entity<AspNetUser>(a => a
                .HasOne(a => a.Address)
                .WithOne(ad => ad.User)
                .HasForeignKey<Address>(ad => ad.UserId));

            // Configure 1-to-1 relationship between Order and Payment
            builder.Entity<Orders>()
                .HasOne(o => o.Payment)
                .WithOne(p => p.Order)
                .HasForeignKey<Payments>(p => p.OrderId);

            // Configure Decimals
            builder.Entity<Orders>()
                .Property(o => o.TotalAmount)
                .HasColumnType("decimal(18,2)");

            builder.Entity<Payments>()
                .Property(p => p.Amount)
                .HasColumnType("decimal(18,2)");

            builder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)");

            // ── Data Seeding ──
            builder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Electronics" },
                new Category { Id = 2, Name = "Fashion" },
                new Category { Id = 3, Name = "Home & Kitchen" }
            );

            builder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Smartphone X", Description = "Latest 5G smartphone with high-res camera.", Price = 799.99m, StockQuantity = 50, CategoryId = 1 },
                new Product { Id = 2, Name = "Wireless Headphones", Description = "Noise-cancelling over-ear headphones.", Price = 199.50m, StockQuantity = 200, CategoryId = 1 },
                new Product { Id = 3, Name = "Cotton T-Shirt", Description = "Comfortable 100% cotton casual t-shirt.", Price = 24.99m, StockQuantity = 150, CategoryId = 2 },
                new Product { Id = 4, Name = "Denim Jeans", Description = "Classic blue slim-fit denim jeans.", Price = 59.99m, StockQuantity = 80, CategoryId = 2 },
                new Product { Id = 5, Name = "Coffee Maker", Description = "Programmable coffee maker with timer.", Price = 89.00m, StockQuantity = 30, CategoryId = 3 }
            );
        }
    }
}
