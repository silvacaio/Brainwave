using Brainwave.API.Data;
using Brainwave.ManagementCourses.Data;
using Brainwave.ManagementCourses.Domain;
using Brainwave.ManagementPayment.Data;
using Brainwave.ManagementStudents.Data;
using Brainwave.ManagementStudents.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Brainwave.API.Configurations
{
    public static class DbMigrationHelpers
    {
        public static void UseDbMigrationHelper(this WebApplication app)
        {
            EnsureSeedData(app).Wait();
        }

        public static async Task EnsureSeedData(WebApplication application)
        {
            var service = application.Services.CreateScope().ServiceProvider;
            await EnsureSeedData(service);
        }

        public static async Task EnsureSeedData(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var courseContext = scope.ServiceProvider.GetRequiredService<CourseContext>();
            var studentContext = scope.ServiceProvider.GetRequiredService<StudentContext>();
            var contextIdentity = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
            var paymentContext = scope.ServiceProvider.GetRequiredService<PaymentContext>();
            var env = scope.ServiceProvider.GetRequiredService<IHostEnvironment>();

            if (env.IsDevelopment() || env.IsEnvironment("Dev"))
            {
                await studentContext.Database.EnsureDeletedAsync();
                await courseContext.Database.EnsureDeletedAsync();
                await contextIdentity.Database.EnsureDeletedAsync();
                await paymentContext.Database.EnsureDeletedAsync();

                await courseContext.Database.MigrateAsync();
                await studentContext.Database.MigrateAsync();
                await contextIdentity.Database.MigrateAsync();
                await paymentContext.Database.MigrateAsync();

                await SeedUsersAndRoles(scope.ServiceProvider);
                await SeedDataInitial(studentContext, courseContext, contextIdentity, paymentContext);
            }
        }

        private static async Task SeedUsersAndRoles(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            var roles = new List<string>() { "ADMIN", "STUDENT" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            var user = new IdentityUser
            {
                Email = "aluno@teste.com",
                EmailConfirmed = true,
                UserName = "aluno@teste.com",
            };

            var userAdmin = new IdentityUser
            {
                Email = "admin@teste.com",
                EmailConfirmed = true,
                UserName = "admin@teste.com",
            };

            await userManager.CreateAsync(user, "Teste@123");
            await userManager.CreateAsync(userAdmin, "Teste@123");

            await userManager.AddToRoleAsync(user, "ALUNO");
            await userManager.AddToRoleAsync(userAdmin, "ADMIN");
        }

        private static async Task SeedDataInitial(StudentContext studentContext, CourseContext courseContext, ApplicationContext dbApplicationContext, PaymentContext paymentContext)
        {
            if (studentContext.Set<Student>().Any() || studentContext.Set<Enrollment>().Any())
                return;

            if (courseContext.Set<Course>().Any() || courseContext.Set<Lesson>().Any())
                return;

            var user = await dbApplicationContext.Users.FirstOrDefaultAsync(x => x.Email == "aluno@teste.com");
            var userAdmin = await dbApplicationContext.Users.FirstOrDefaultAsync(x => x.Email == "admin@teste.com");
        }
    }
}
