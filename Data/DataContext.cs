using Microsoft.EntityFrameworkCore;
using E_commerce.Models;
using E_commerce.Abstractions;

namespace E_commerce.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Subcategory> Subcategories { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<PasswordResetToken> PasswordResetTokens { get; set; }
        public DbSet<Click> Clicks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            
            modelBuilder.Entity<CartItem>(entity =>
            {
                entity.HasKey(c => c.Id);

                
                entity.HasIndex(c => new { c.UserId, c.ProductId })
                      .HasDatabaseName("IX_CartItems_UserId_ProductId");

               
                entity.Property(c => c.Price)
                      .HasColumnType("decimal(18,2)");

                entity.Property(c => c.Name)
                      .IsRequired()
                      .HasMaxLength(200);

                entity.Property(c => c.UserId)
                      .IsRequired();

                entity.Property(c => c.ProductId)
                      .IsRequired();

                entity.Property(c => c.Quantity)
                      .IsRequired()
                      .HasDefaultValue(1);

                entity.Property(c => c.QuantityAvailable)
                      .IsRequired()
                      .HasDefaultValue(0);

                entity.Property(c => c.AddedAt)
                      .IsRequired()
                      .HasDefaultValueSql("GETUTCDATE()");
            });

           
            var itEquipment = It_EquipmentAbstractions.GetProducts();
            var appliances = AppliancesAbstractions.GetProducts();
            var mobileDevices = Mobile_devicesAbstractions.GetProducts();

            modelBuilder.Entity<Product>().HasData(itEquipment);
            modelBuilder.Entity<Product>().HasData(appliances);
            modelBuilder.Entity<Product>().HasData(mobileDevices);
        }
    }
}