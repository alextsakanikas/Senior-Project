using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Tracking_Events.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>().HasOne(a => a.Venue).WithOne(v => v.User).HasForeignKey<Venue>(v => v.UserID).OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Request>().HasOne(r => r.Venue).WithMany(v => v.Requests).OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Event>().HasOne(e => e.Venue).WithMany(v => v.Events).OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Review>().HasOne(r => r.Venue).WithMany(v => v.Reviews).OnDelete(DeleteBehavior.Cascade);
        }

        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<Venue> Venue { get; set; }
        public DbSet<Request> Request { get; set; }
        public DbSet<Event> Event { get; set; }
        public DbSet<Review> Review { get; set; }
    }
}
