using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Voxo.Models;

namespace Voxo.DAL
{
    public class VoxoContext:IdentityDbContext
    {
        public VoxoContext(DbContextOptions<VoxoContext> options):base(options) 
        { 
        }

        public DbSet<Banner> Banners { get; set; }

        public DbSet<Setting> Settings { get; set; }

        public DbSet<Service> Services { get; set; }

        public DbSet<TeamMember> TeamMembers { get; set; }

        public DbSet<FAQ> FAQs { get; set; }

        public DbSet<ContactUsRequest> ContactUsRequests { get; set; }

        public DbSet<Size> Sizes { get; set; }  

        public DbSet<Product> Products { get; set; }

        public DbSet<Brand> Brands { get; set; }

        public DbSet<ProductSize> ProductSizes { get; set; }

        public DbSet<Slider> Sliders { get; set; }

        public DbSet<Tag> Tags { get; set; }

        public DbSet<ProductTag> ProductTags { get; set; }

        public DbSet<ProductImage> ProductImages { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<AppUser> AppUsers { get; set; }

        public DbSet<UserCartItem> UserCartItems { get; set; }

        public DbSet<UserWishlistItem> Wishlists { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderItem> OrderItems { get; set; }

        public DbSet<ProductReview> ProductReviews { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Setting>().HasKey(x => x.Key);
            builder.Entity<ProductTag>().HasKey(x => new { x.TagId,x.ProductId });
            builder.Entity<ProductSize>().HasKey(x=> new {x.SizeId,x.ProductId});



            base.OnModelCreating(builder);
        }


    }
}
