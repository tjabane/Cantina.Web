using Cantina.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace Cantina.Infrastructure.SQL
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
        }
    }
}
