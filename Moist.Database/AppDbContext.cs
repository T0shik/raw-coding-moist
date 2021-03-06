﻿using Microsoft.EntityFrameworkCore;
using Moist.Core.Models;

namespace Moist.Database
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Code> Codes { get; set; }
        public DbSet<Shop> Shops { get; set; }
        public DbSet<Schema> Schemas { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Employee>().HasKey(x => new {x.ShopId, x.UserId});

            builder.Entity<Shop>().HasMany(x => x.Codes)
                   .WithOne(x => x.Shop)
                   .HasForeignKey(x => x.ShopId)
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }
}