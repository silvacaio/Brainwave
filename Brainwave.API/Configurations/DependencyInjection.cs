using Brainwave.Core.Communication.Mediator;
using Brainwave.Core.Data.EventSourcing;
using Brainwave.Core.Extensions;
using Brainwave.Core.Messages.CommonMessages.Notifications;
using Brainwave.ManagementCourses.Application.Commands.Course;
using Brainwave.ManagementCourses.Application.Commands.Lesson;
using Brainwave.ManagementCourses.Application.Queries;
using Brainwave.ManagementCourses.Data.Repository;
using Brainwave.ManagementCourses.Domain;
using Brainwave.ManagementPayment.AntiCorruption;
using Brainwave.ManagementPayment.Application;
using Brainwave.ManagementPayment.Application.Commands;
using Brainwave.ManagementPayment.Business;
using Brainwave.ManagementPayment.Data.Repository;
using Brainwave.ManagementStudents.Application.Commands.Enrollment;
using Brainwave.ManagementStudents.Application.Commands.StudentLesson;
using Brainwave.ManagementStudents.Application.Commands.User;
using Brainwave.ManagementStudents.Application.Queries;
using Brainwave.ManagementStudents.Data.Repository;
using Brainwave.ManagementStudents.Domain;
using EventSourcing;
using MediatR;

namespace Brainwave.API.Configurations
{
    public static class DependencyInjection
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            // Mediator
            services.AddScoped<IMediatorHandler, MediatorHandler>();

            // Notifications
            services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();

            services.AddScoped<ICommandValidator, CommandValidator>();

            // Event Sourcing
            services.AddSingleton<IEventStoreService, EventStoreService>();
            services.AddSingleton<IEventSourcingRepository, EventSourcingRepository>();

            //Course
            services.AddScoped<ICourseRepository, CourseRepository>();
            services.AddScoped<ICourseQueries, CourseQueries>();


            services.AddScoped<IRequestHandler<AddCourseCommand, bool>, CourseCommandHandler>();
            services.AddScoped<IRequestHandler<UpdateCourseCommand, bool>, CourseCommandHandler>();
            services.AddScoped<IRequestHandler<DeleteCourseCommand, bool>, CourseCommandHandler>();

            services.AddScoped<IRequestHandler<AddLessonCommand, bool>, LessonCommandHandler>();


            //Student
            services.AddScoped<IStudentRepository, StudentRepository>();
            services.AddScoped<IStudentQueries, StudentQueries>();

            services.AddScoped<IRequestHandler<AddEnrollmentCommand, bool>, EnrollmentCommandHandler>();
            services.AddScoped<IRequestHandler<EnrollmentPaidCommand, bool>, EnrollmentCommandHandler>();
            services.AddScoped<IRequestHandler<FinishEnrollmentCommand, bool>, EnrollmentCommandHandler>();

            services.AddScoped<IRequestHandler<FinishLessonCommand, bool>, StudentLessonCommandHandler>();

            services.AddScoped<IRequestHandler<AddStudentCommand, bool>, UserCommandHandler>();
            services.AddScoped<IRequestHandler<AddAdminCommand, bool>, UserCommandHandler>();

            //Payment

            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<ICreditCardPaymentFacade, CreditCardPaymentFacade>();
            services.AddScoped<IRequestHandler<MakePaymentCommand, bool>, PaymentCommandHandler>();


        }
    }
}
