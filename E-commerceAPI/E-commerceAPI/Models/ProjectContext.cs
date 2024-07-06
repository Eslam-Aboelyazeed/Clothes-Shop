using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace E_commerceAPI.Models
{
    public class ProjectContext:IdentityDbContext<ApplicationUser>
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductReviews> ProductReviews { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderProducts> OrderProducts { get; set; }
        public DbSet<Coupon> Coupons { get; set; }
        public DbSet<OrderCoupon> OrderCoupons { get; set; }
        public DbSet<SpecialOffer> SpecialOffers { get; set; }
        public DbSet<ContactUs> ContactUs { get; set; }

        public ProjectContext():base() { }
        public ProjectContext(DbContextOptions<ProjectContext> options):base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ContactUs>()
                .HasOne(c => c.user)
                .WithMany(u => u.contactUsMessages)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Order>()
                .HasOne(c => c.user)
                .WithMany(u => u.orders)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<ProductReviews>()
                .HasOne(c => c.user)
                .WithMany(u => u.productReviews)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<ProductReviews>()
                .HasOne(c => c.product)
                .WithMany(p => p.productReviews)
                .HasForeignKey(c => c.ProductId)
                .OnDelete(DeleteBehavior.NoAction);


            builder.Entity<SpecialOffer>()
                .HasOne(s => s.product)
                .WithMany(p => p.specialOffers)
                .HasForeignKey(s => s.Id)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<OrderCoupon>()
                .HasOne(o => o.order)
                .WithMany(o => o.orderCoupons)
                .HasForeignKey(o => o.OrderId)
                .OnDelete(DeleteBehavior.NoAction);


            builder.Entity<OrderCoupon>()
                .HasOne(o => o.coupon)
                .WithMany(c => c.orderCoupons)
                .HasForeignKey(o => o.CouponId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<OrderProducts>()
                .HasOne(o => o.order)
                .WithMany(o => o.orderProducts)
                .HasForeignKey(o => o.OrderId)
                .OnDelete(DeleteBehavior.NoAction);


            builder.Entity<OrderProducts>()
                .HasOne(o => o.product)
                .WithMany(p => p.orderProducts)
                .HasForeignKey(o => o.ProductId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<OrderCoupon>().HasKey("OrderId", "CouponId");

            builder.Entity<OrderProducts>().HasKey("OrderId", "ProductId");

            builder.Entity<ProductReviews>().HasKey("ProductId", "UserId");
        }

    }
}
