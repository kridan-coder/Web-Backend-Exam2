using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BackendTest2.Models;

namespace BackendTest2.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<HogTask> HogTasks { get; set; }

        public DbSet<HogTaskUser> TaskUsers { get; set; }

        public DbSet<Theme> Themes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            builder.Entity<HogTaskUser>().HasOne(x => x.User).WithMany().OnDelete(DeleteBehavior.Restrict);


            builder.Entity<HogTaskUser>()
                .HasKey(x => new { x.HogTaskId, x.UserId });
        }
    }
}
