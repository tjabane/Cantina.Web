using Cantina.Domain.Entities;
using Cantina.Infrastructure.Database.Configuration;
using Cantina.Infrastructure.Database.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Reflection.Emit;


namespace Cantina.Infrastructure.Database
{
    public class CantinaDbContext(DbContextOptions<CantinaDbContext> options) : IdentityDbContext<IdentityUser>(options)
    {
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new ActionConfiguration());
            builder.ApplyConfiguration(new MenuAuditConfiguration());
            builder.ApplyConfiguration(new MenuItemConfiguration());
            builder.ApplyConfiguration(new MenuItemTypeConfiguration());
            builder.ApplyConfiguration(new ReviewConfiguration());

            builder.Entity<ApplicationUser>(entity =>
            {
                entity.Property(e => e.FullName).HasMaxLength(256);
            });

            SeedUsersRoles seedUsersRoles = new();
            builder.Entity<IdentityRole>().HasData(seedUsersRoles.Roles);
            builder.Entity<ApplicationUser>().HasData(seedUsersRoles.Users);
            builder.Entity<IdentityUserRole<string>>().HasData(seedUsersRoles.UserRoles);
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.ConfigureWarnings(warnings =>
                warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
        }
    }
}
