using Brainwave.API.Data;
using Brainwave.ManagementCourses.Data;
using Brainwave.ManagementPayment.Data;
using Brainwave.ManagementStudents.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Brainwave.API.Configurations
{
    public static class DbContextConfiguration
    {
        public static WebApplicationBuilder AddDbContextConfiguration(this WebApplicationBuilder builder)
        {
            if (builder.Environment.IsProduction())
            {
                builder.Services.AddDbContext<CourseContext>(opt =>
                {
                    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
                });
                builder.Services.AddDbContext<StudentContext>(opt =>
                {
                    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
                }, ServiceLifetime.Transient);
                builder.Services.AddDbContext<ApplicationContext>(opt =>
                {
                    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
                });
                builder.Services.AddDbContext<PaymentContext>(opt =>
                {
                    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
                });
            }
            else
            {
                builder.Services.AddDbContext<CourseContext>(opt =>
                {
                    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
                });
                builder.Services.AddDbContext<StudentContext>(opt =>
                {
                    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
                }, ServiceLifetime.Transient);
                builder.Services.AddDbContext<ApplicationContext>(opt =>
                {
                    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
                });
                builder.Services.AddDbContext<PaymentContext>(opt =>
                {
                    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
                });
            }
            builder.Services.AddIdentity<IdentityUser, IdentityRole>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationContext>()
                .AddDefaultTokenProviders();
            return builder;
        }
    }
