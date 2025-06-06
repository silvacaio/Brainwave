using Brainwave.API.Data;
using Brainwave.ManagementStudents.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace Brainwave.API.Configurations
{
    public static class ApiConfiguration
    {
        public static WebApplicationBuilder AddApiConfiguration(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<ApplicationContext>(options =>
              options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            //TODO resolver erro
            builder.Services.AddDefaultIdentity<IdentityUser<Guid>>()
            .AddRoles<IdentityRole<Guid>>()
              .AddEntityFrameworkStores<ApplicationContext>()
              .AddSignInManager()
              .AddRoleManager<RoleManager<IdentityRole<Guid>>>()
              .AddDefaultTokenProviders();


            builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
          .AddJsonFile("appsettings.json", true, true)
          .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
          .AddEnvironmentVariables();

            builder.Services.AddControllers()
        .ConfigureApiBehaviorOptions(opt => opt.SuppressModelStateInvalidFilter = true);

            builder.Services.AddHttpContextAccessor();

            builder.Services.AddCors(opt => opt.AddPolicy("*", b =>
            {
                b.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));

            return builder;

        }
    }
}
