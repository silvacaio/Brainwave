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
            var courseContext = scope.ServiceProvider.GetRequiredService<CoursesContext>();
            var studentContext = scope.ServiceProvider.GetRequiredService<StudentsContext>();
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

                await SeedUsersAndRoles(studentContext, contextIdentity);
                await SeedDataInitial(studentContext, courseContext, contextIdentity, paymentContext);
            }
        }

        private static async Task SeedUsersAndRoles(StudentsContext context, ApplicationContext contextIdentity)
        {
            if (context.Students.Any()) return;

            #region ADMIN SEED
            var ADMIN_ROLE_ID = Guid.NewGuid();
            await contextIdentity.Roles.AddAsync(new IdentityRole<Guid>
            {
                Name = "ADMIN",
                NormalizedName = "ADMIN",
                Id = ADMIN_ROLE_ID,
                ConcurrencyStamp = ADMIN_ROLE_ID.ToString()
            });

            var STUDENT_ROLE_ID = Guid.NewGuid();
            await contextIdentity.Roles.AddAsync(new IdentityRole<Guid>
            {
                Name = "STUDENT",
                NormalizedName = "STUDENT",
                Id = STUDENT_ROLE_ID,
                ConcurrencyStamp = STUDENT_ROLE_ID.ToString()
            });

            var ADMIN_ID = Guid.NewGuid();
            var adminUser = new IdentityUser<Guid>
            {
                Id = ADMIN_ID,
                Email = "admin@brainwave.com",
                EmailConfirmed = true,
                UserName = "admin@brainwave.com",
                NormalizedUserName = "admin@brainwave.com".ToUpper(),
                NormalizedEmail = "admin@brainwave.com".ToUpper(),
                LockoutEnabled = true,
                SecurityStamp = ADMIN_ROLE_ID.ToString(),
            };

            //set user password
            PasswordHasher<IdentityUser<Guid>> ph = new PasswordHasher<IdentityUser<Guid>>();
            adminUser.PasswordHash = ph.HashPassword(adminUser, "Teste@123");
            await contextIdentity.Users.AddAsync(adminUser);

            var user = Student.StudentFactory.CreateAdmin(adminUser.Id, adminUser.UserName);
            await context.Students.AddAsync(user);

            await contextIdentity.UserRoles.AddAsync(new IdentityUserRole<Guid>
            {
                RoleId = ADMIN_ROLE_ID,
                UserId = ADMIN_ID
            });

            context.SaveChanges();
            #endregion

            #region NON-ADMIN USERS SEED
            var user1Id = Guid.NewGuid();
            var user1 = new IdentityUser<Guid>
            {
                Id = user1Id,
                Email = "user1@brainwave.com",
                EmailConfirmed = true,
                UserName = "user1@brainwave.com",
                NormalizedUserName = "user1@brainwave.com".ToUpper(),
                NormalizedEmail = "user1@brainwave.com".ToUpper(),
                LockoutEnabled = true,
                SecurityStamp = user1Id.ToString(),
            };
            user1.PasswordHash = ph.HashPassword(user1, "Teste@123");
            await contextIdentity.Users.AddAsync(user1);

            var systemUser1 = Student.StudentFactory.CreateStudent(user1.Id, user1.UserName);
            await context.Students.AddAsync(systemUser1);

            var user2Id = Guid.NewGuid();
            var user2 = new IdentityUser<Guid>
            {
                Id = user2Id,
                Email = "user2@brainwave.com",
                EmailConfirmed = true,
                UserName = "user2@brainwave.com",
                NormalizedUserName = "user2@brainwave.com".ToUpper(),
                NormalizedEmail = "user2@brainwave.com".ToUpper(),
                LockoutEnabled = true,
                SecurityStamp = user2Id.ToString(),
            };
            user2.PasswordHash = ph.HashPassword(user2, "Teste@123");
            await contextIdentity.Users.AddAsync(user2);

            await contextIdentity.UserRoles.AddAsync(new IdentityUserRole<Guid>
            {
                RoleId = STUDENT_ROLE_ID,
                UserId = user1Id
            });

            await contextIdentity.UserRoles.AddAsync(new IdentityUserRole<Guid>
            {
                RoleId = STUDENT_ROLE_ID,
                UserId = user2Id
            });


            var systemUser2 = Student.StudentFactory.CreateStudent(user2.Id, user2.UserName);
            await context.Students.AddAsync(systemUser2);

            context.SaveChanges();
            #endregion
        }

        private static async Task SeedDataInitial(StudentsContext studentContext, CoursesContext courseContext, ApplicationContext dbApplicationContext, PaymentContext paymentContext)
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
