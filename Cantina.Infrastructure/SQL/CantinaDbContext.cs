using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace Cantina.Infrastructure.SQL
{
    public class CantinaDbContext(DbContextOptions<CantinaDbContext> options) : IdentityDbContext<IdentityUser>(options)
    {
    }
}
