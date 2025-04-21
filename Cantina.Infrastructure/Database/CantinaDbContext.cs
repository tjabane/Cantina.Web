using Cantina.Domain.Entities;
using Cantina.Infrastructure.Database.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace Cantina.Infrastructure.Database
{
    public class CantinaDbContext(DbContextOptions<CantinaDbContext> options) : IdentityDbContext<IdentityUser>(options)
    {
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<ApplicationUser>(entity =>
            {
                entity.Property(e => e.FullName).HasMaxLength(256);
            });


            SeedUsersRoles seedUsersRoles = new();
            builder.Entity<IdentityRole>().HasData(seedUsersRoles.Roles);
            builder.Entity<ApplicationUser>().HasData(seedUsersRoles.Users);
            builder.Entity<IdentityUserRole<string>>().HasData(seedUsersRoles.UserRoles);
        }
    }
}
