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
        public static WebApplicationBuilder RegisterServices(this WebApplicationBuilder builder)
        {
            // Mediator
            builder.Services.AddScoped<IMediatorHandler, MediatorHandler>();

            // Notifications
            builder.Services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();

            builder.Services.AddScoped<ICommandValidator, CommandValidator>();

            // Event Sourcing
            builder.Services.AddSingleton<IEventStoreService, EventStoreService>();
            builder.Services.AddSingleton<IEventSourcingRepository, EventSourcingRepository>();

            //Course
            builder.Services.AddScoped<ICourseRepository, CourseRepository>();
            builder.Services.AddScoped<ICourseQueries, CourseQueries>();


            builder.Services.AddScoped<IRequestHandler<AddCourseCommand, bool>, CourseCommandHandler>();
            builder.Services.AddScoped<IRequestHandler<UpdateCourseCommand, bool>, CourseCommandHandler>();
            builder.Services.AddScoped<IRequestHandler<DeleteCourseCommand, bool>, CourseCommandHandler>();

            builder.Services.AddScoped<IRequestHandler<AddLessonCommand, bool>, LessonCommandHandler>();


            //Student
            builder.Services.AddScoped<IStudentRepository, StudentRepository>();
            builder.Services.AddScoped<IStudentQueries, StudentQueries>();

            builder.Services.AddScoped<IRequestHandler<AddEnrollmentCommand, bool>, EnrollmentCommandHandler>();
            builder.Services.AddScoped<IRequestHandler<EnrollmentPaidCommand, bool>, EnrollmentCommandHandler>();
            builder.Services.AddScoped<IRequestHandler<FinishEnrollmentCommand, bool>, EnrollmentCommandHandler>();

            builder.Services.AddScoped<IRequestHandler<FinishLessonCommand, bool>, StudentLessonCommandHandler>();

            builder.Services.AddScoped<IRequestHandler<AddStudentCommand, bool>, UserCommandHandler>();
            builder.Services.AddScoped<IRequestHandler<AddAdminCommand, bool>, UserCommandHandler>();

            //Payment

            builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
            builder.Services.AddScoped<ICreditCardPaymentFacade, CreditCardPaymentFacade>();
            builder.Services.AddScoped<IRequestHandler<MakePaymentCommand, bool>, PaymentCommandHandler>();

            return builder;

        }
    }
}
