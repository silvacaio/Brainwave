using Brainwave.API.Data;
using Brainwave.Core.Enums;
using Brainwave.ManagementCourses.Data;
using Brainwave.ManagementPayment.Data;
using Brainwave.ManagementStudents.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;

namespace Brainwave.API.Configurations
{
    public static class DbContextConfiguration
    {
        public static WebApplicationBuilder AddDbContextConfiguration(this WebApplicationBuilder builder, EDatabases databases)
        {
            switch (databases)
            {
                case EDatabases.SQLServer:
                    builder.Services.AddDbContext<CoursesContext>(opt =>
                {
                    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
                });
                    builder.Services.AddDbContext<StudentsContext>(opt =>
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
                    break;


                case EDatabases.SQLite:

                    builder.Services.AddDbContext<CoursesContext>(opt =>
                    {
                        opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
                    });
                    builder.Services.AddDbContext<StudentsContext>(opt =>
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
                    break;
                default:
                    throw new ArgumentException($"Banco de dados {databases} não suportado.");

            }

            builder.Services.AddDefaultIdentity<IdentityUser<Guid>>()
                   .AddRoles<IdentityRole<Guid>>()
                   .AddEntityFrameworkStores<ApplicationContext>()
                   .AddSignInManager()
                   .AddRoleManager<RoleManager<IdentityRole<Guid>>>()
                   .AddDefaultTokenProviders();

            return builder;
        }
    }
}
