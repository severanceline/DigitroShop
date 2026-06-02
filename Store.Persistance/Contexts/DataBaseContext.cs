using Microsoft.EntityFrameworkCore;
using Store.Application.Interfaces.Contexts;
using Store.Common.Roles;
using Store.Domain.Entities.Carts;
using Store.Domain.Entities.Finances;
using Store.Domain.Entities.HomePage;
using Store.Domain.Entities.Orders;
using Store.Domain.Entities.Products;
using Store.Domain.Entities.Users;
using Store.Persistance.Migrations;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RequestPay = Store.Domain.Entities.Finances.RequestPay;


namespace Store.Persistance.Contexts
{
    public class DataBaseContext : DbContext , IDataBaseContext 
    {
        public DataBaseContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users {get; set;}
        public DbSet<Role> Roles {get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<UserInRole> UserInRoles { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductFeatures> ProductFeatures { get; set; }
        public DbSet<ProductImages> ProductImages { get; set; }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<HomePageImages> HomePageImages { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<RequestPay> RequestPays { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        //به این شک دارم که بودنش اینجا مسکلس اجاد نمکنه
        public Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess)
        {
            throw new NotImplementedException();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            SeedData(modelBuilder);
            //اعمال ایندکس و غیر تکراری بودن ایمیل
            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
            ApplyQueryFilter(modelBuilder);

            //اگر خواستیم یوزری رو که اردر داره پاک نیم نمیزاره 
            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)          
                .WithMany(u => u.Orders)      
                .HasForeignKey(o => o.UserId) 
                .OnDelete(DeleteBehavior.NoAction);

            // اگر خواستیم ریکوعست پی رو پاک کنیم که اردر داره نمیزاره 
            modelBuilder.Entity<Order>()
                .HasOne(o => o.RequestPay)
                .WithMany(r => r.Orders)
                .HasForeignKey(o => o.RequestPayId)
                .OnDelete(DeleteBehavior.NoAction);
        }
        private void SeedData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasData(new Role { Id = 1, Name = nameof(UserRoles.Admin) });
            modelBuilder.Entity<Role>().HasData(new Role { Id = 2, Name = nameof(UserRoles.Operator) });
            modelBuilder.Entity<Role>().HasData(new Role { Id = 3, Name = nameof(UserRoles.Customer) });
        }
        private void ApplyQueryFilter(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasQueryFilter(p => !p.IsRemoved);
            modelBuilder.Entity<UserInRole>().HasQueryFilter(ur => !ur.user.IsRemoved);
            modelBuilder.Entity<Category>().HasQueryFilter(p => !p.IsRemoved);
            modelBuilder.Entity<Product>().HasQueryFilter(p => !p.IsRemoved);
            modelBuilder.Entity<ProductFeatures>().HasQueryFilter(p => !p.IsRemoved);
            modelBuilder.Entity<ProductImages>().HasQueryFilter(p => !p.IsRemoved);
            modelBuilder.Entity<Slider>().HasQueryFilter(p => !p.IsRemoved);
            modelBuilder.Entity<HomePageImages>().HasQueryFilter(p => !p.IsRemoved);
            modelBuilder.Entity<Cart>().HasQueryFilter(p => !p.IsRemoved);
            modelBuilder.Entity<CartItem>().HasQueryFilter(p => !p.IsRemoved);
            modelBuilder.Entity<RequestPay>().HasQueryFilter(p => !p.IsRemoved);
            modelBuilder.Entity<Order>().HasQueryFilter(p => !p.IsRemoved);
            modelBuilder.Entity<OrderDetail>().HasQueryFilter(p => !p.IsRemoved);
        }
    }   
}
    