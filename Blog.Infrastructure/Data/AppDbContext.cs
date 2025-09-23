using Blog.Core.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Database Tables
        public DbSet<Post> Posts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<Post>(entity =>
            //{
            //    entity.Property(e => e.Content).IsRequired();
            //    // PK
            //    entity.HasKey(e => e.Id);

            //    // unique
            //    entity.HasIndex(e => e.Title).IsUnique();
            //});
            //modelBuilder.Entity<User>().HasIndex(e => e.Email).IsUnique();
            // relations

            // 1 user => M Posts
            //modelBuilder.Entity<Post>()
            //    .HasOne(p => p.User)
            //    .WithMany(u => u.Posts)
            //    .HasForeignKey(e => e.UserId)
            //    .OnDelete(DeleteBehavior.Restrict);

            //modelBuilder.Entity<Post>()
            //    .HasMany(p => p.Comments)
            //    .WithMany(c => c.Post)
            //    .UseEntity(o => o.ToTable("TableName"));
        }

    }
}
