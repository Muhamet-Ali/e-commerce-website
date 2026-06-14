using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using E_StoreMVCTemplate.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace E_StoreMVCTemplate.Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Görseldeki tüm Entity'ler için DbSet tanımlamaları
        public DbSet<Category> Categories { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; } // Eğer Category içinde tutmuyorsan ayrı tablo
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImages> ProductImages { get; set; }
        public DbSet<HeaderImages> HeaderImages { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Discountcs> Discounts { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<Review> Reviews { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Decimal hassasiyeti
            foreach (var property in builder.Model.GetEntityTypes()
                .SelectMany(t => t.GetProperties())
                .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
            {
                property.SetColumnType("decimal(18,2)");
            }

            // Category → SubCategory (Cascade)
            builder.Entity<Category>()
                .HasMany(c => c.SubCategories)
                .WithOne(s => s.Category)
                .HasForeignKey(s => s.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            // Category → Product (NoAction - döngüyü kırar)
            builder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.NoAction);

            // SubCategory → Product (NoAction - döngüyü kırar)
            builder.Entity<Product>()
                .HasOne(p => p.SubCategory)
                .WithMany(s => s.Products)
                .HasForeignKey(p => p.SubCategoryId)
                .OnDelete(DeleteBehavior.NoAction);

            // Product → ProductImages (Cascade)
            builder.Entity<Product>()
                .HasMany(p => p.Images)
                .WithOne(i => i.Products)
                .HasForeignKey(i => i.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            // Product → Discount (SetNull - indirim silinirse ürün kalır)
            builder.Entity<Product>()
                .HasOne(p => p.Discountcs)
                .WithMany(d => d.Products)
                .HasForeignKey(p => p.DiscountId)
                .OnDelete(DeleteBehavior.SetNull);

            // Cart → CartItems (Cascade)
            builder.Entity<Cart>()
                .HasMany(c => c.CartItems)
                .WithOne(i => i.Cart)
                .HasForeignKey(i => i.CartId)
                .OnDelete(DeleteBehavior.Cascade);

            // AppUser → Cart (Cascade)
            builder.Entity<AppUser>()
                .HasOne(u => u.Cart)
                .WithOne(c => c.User)
                .HasForeignKey<Cart>(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Payment>()
                .HasOne(p => p.User)
                .WithMany()
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Payment>()
                .HasOne(p => p.Cart)
                .WithMany()
                .HasForeignKey(p => p.CartId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
