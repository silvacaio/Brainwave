using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Brainwave.API.Data
{
    public class ApplicationContext(DbContextOptions<ApplicationContext> options) : IdentityDbContext(options)
    {
    }
}
