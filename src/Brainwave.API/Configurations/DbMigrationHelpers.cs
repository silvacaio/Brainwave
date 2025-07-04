using Brainwave.API.Data;
using Brainwave.ManagementCourses.Data;
using Brainwave.ManagementCourses.Domain;
using Brainwave.ManagementCourses.Domain.ValueObjects;
using Brainwave.ManagementPayment.Application;
using Brainwave.ManagementPayment.Application.ValueObjects;
using Brainwave.ManagementPayment.Data;
using Brainwave.ManagementStudents.Data;
using Brainwave.ManagementStudents.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Runtime.Intrinsics.X86;
using System.Transactions;
using static Brainwave.ManagementStudents.Domain.Enrollment;

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

            if (env.IsDevelopment() || env.IsEnvironment("Testing"))
            {
                await studentContext.Database.EnsureDeletedAsync();
                await courseContext.Database.EnsureDeletedAsync();
                await contextIdentity.Database.EnsureDeletedAsync();
                await paymentContext.Database.EnsureDeletedAsync();

                await courseContext.Database.MigrateAsync();
                await studentContext.Database.MigrateAsync();
                await contextIdentity.Database.MigrateAsync();
                await paymentContext.Database.MigrateAsync();

                await SeedUsersAndRoles(contextIdentity);
                await SeedDataInitial(studentContext, courseContext, contextIdentity, paymentContext);
            }
        }

        private static async Task SeedUsersAndRoles(ApplicationContext contextIdentity)
        {
            if (contextIdentity.Users.Any()) return;

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


            await contextIdentity.UserRoles.AddAsync(new IdentityUserRole<Guid>
            {
                RoleId = ADMIN_ROLE_ID,
                UserId = ADMIN_ID
            });

            #endregion

            #region NON-ADMIN USERS SEED
            var user1Id = Guid.NewGuid();
            var user1 = new IdentityUser<Guid>
            {
                Id = user1Id,
                Email = "aluno1@brainwave.com",
                EmailConfirmed = true,
                UserName = "aluno1@brainwave.com",
                NormalizedUserName = "aluno1@brainwave.com".ToUpper(),
                NormalizedEmail = "aluno1@brainwave.com".ToUpper(),
                LockoutEnabled = true,
                SecurityStamp = user1Id.ToString(),
            };
            user1.PasswordHash = ph.HashPassword(user1, "Teste@123");
            await contextIdentity.Users.AddAsync(user1);



            var user2Id = Guid.NewGuid();
            var user2 = new IdentityUser<Guid>
            {
                Id = user2Id,
                Email = "aluno2@brainwave.com",
                EmailConfirmed = true,
                UserName = "aluno2@brainwave.com",
                NormalizedUserName = "aluno2@brainwave.com".ToUpper(),
                NormalizedEmail = "aluno2@brainwave.com".ToUpper(),
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

            await contextIdentity.SaveChangesAsync();

            #endregion
        }

        private static async Task SeedDataInitial(StudentsContext studentContext, CoursesContext courseContext, ApplicationContext dbApplicationContext, PaymentContext paymentContext)
        {
            if (studentContext.Set<Student>().Any() || studentContext.Set<Enrollment>().Any())
                return;

            if (courseContext.Set<Course>().Any() || courseContext.Set<Lesson>().Any())
                return;

            var userStudent = await dbApplicationContext.Users.FirstOrDefaultAsync(x => x.Email == "aluno1@brainwave.com");
            var userStudent2 = await dbApplicationContext.Users.FirstOrDefaultAsync(x => x.Email == "aluno2@brainwave.com");
            var userAdmin = await dbApplicationContext.Users.FirstOrDefaultAsync(x => x.Email == "admin@brainwave.com");

            var admin = Student.StudentFactory.CreateAdmin(userAdmin.Id, userAdmin.UserName);
            var student = Student.StudentFactory.CreateStudent(userStudent.Id, userStudent.UserName);
            var student2 = Student.StudentFactory.CreateStudent(userStudent2.Id, userStudent2.UserName);


            var course = Course.CourseFactory.New(".NET", 100, new Syllabus("Test content", 30, "English"));
            var lesson1 = Lesson.LessonFactory.New(course.Id, "Lesson 1", "Test", "pdf");
            var lesson2 = Lesson.LessonFactory.New(course.Id, "Lesson 2", "Test", "ADB");
            var lesson3 = Lesson.LessonFactory.New(course.Id, "Lesson 3", "Test", "SDASDASDASDASDAD");
            course.AddLesson(lesson1);
            course.AddLesson(lesson2);
            course.AddLesson(lesson3);

            var course2 = Course.CourseFactory.New("Unit Tests", 50, new Syllabus("Test content", 10, "English"));
            var lesson4 = Lesson.LessonFactory.New(course.Id, "Lesson 4", "Test", "pdf");
            var lesson5 = Lesson.LessonFactory.New(course.Id, "Lesson 5", "Test", "ADB");
            course2.AddLesson(lesson4);
            course2.AddLesson(lesson5);

            var course3 = Course.CourseFactory.New("Test", 100, new Syllabus("Test content", 30, "English"));



            // Enrollment
            var enrollmentActive2 = EnrollmentActive.Create(student2.Id, course2.Id);

            var enrollmentPendingPayment = EnrollmentPendingPayment.Create(student2.Id, course.Id);

            var enrollmentActive = EnrollmentActive.Create(student.Id, course.Id);

            var enrollmentDone = EnrollmentDone.Create(student.Id, course2.Id);


            //StudentLesson 1

            var studentLesson1 = StudentLesson.StudentLessonFactory.Create(student.Id, lesson1.CourseId, lesson1.Id);
            var studentLesson2 = StudentLesson.StudentLessonFactory.Create(student.Id, lesson2.CourseId, lesson2.Id);
            var studentLesson3 = StudentLesson.StudentLessonFactory.Create(student.Id, lesson3.CourseId, lesson3.Id);

            // StudentLesson 2
            var studentLesson4 = StudentLesson.StudentLessonFactory.Create(student2.Id, lesson4.CourseId, lesson4.Id);

            //Certificate
            var certificate = Certificate.CertificateFactory.Create(student.Name, course.Title, enrollmentDone.Id, student.Id);

            // Payment
            var creditCard = CreditCard.CreditCardFactory.Create("11111111", "New credit card", DateTime.Today.AddYears(1), "123");
            var payment = Payment.PaymentFactory.Create(enrollmentDone.Id, course.Value);
            payment.AddCreditCard(creditCard);

            //PaymentTransaction
            var paymentTransaction = PaymentTransaction.PaymentTransactionFactory.Paid(enrollmentDone.Id, payment.Id, course.Value);

            await courseContext.Courses.AddRangeAsync(course, course2, course3);

            await courseContext.Lessons.AddRangeAsync(lesson1, lesson2, lesson3, lesson4, lesson5);

            await studentContext.Students.AddRangeAsync(admin, student, student2);

            await studentContext.Enrollments.AddRangeAsync(enrollmentDone, enrollmentPendingPayment, enrollmentActive, enrollmentActive2);

            await studentContext.StudentLessons.AddRangeAsync(studentLesson1, studentLesson2, studentLesson3, studentLesson4);

            await studentContext.Certificates.AddRangeAsync(certificate);

            await paymentContext.Payments.AddRangeAsync(payment);

            await paymentContext.PaymentTransactions.AddRangeAsync(paymentTransaction);

            await courseContext.SaveChangesAsync();
            await studentContext.SaveChangesAsync();
            await paymentContext.SaveChangesAsync();
        }
    }
}
