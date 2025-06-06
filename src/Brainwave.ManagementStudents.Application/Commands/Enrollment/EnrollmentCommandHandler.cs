using Brainwave.Core.Extensions;
using Brainwave.Core.Messages.CommonMessages.Notifications;
using Brainwave.ManagementStudents.Application.Events;
using Brainwave.ManagementStudents.Domain;
using MediatR;
using static Brainwave.ManagementStudents.Domain.Enrollment;

namespace Brainwave.ManagementStudents.Application.Commands.Enrollment
{
    public class EnrollmentCommandHandler :
        IRequestHandler<AddEnrollmentCommand, bool>,
        IRequestHandler<EnrollmentPaidCommand, bool>,
        IRequestHandler<FinishEnrollmentCommand, bool>
    {
        private readonly ICommandValidator _commandValidator;
        private readonly IStudentRepository _studentRepository;
        private readonly IMediator _mediator;

        public EnrollmentCommandHandler(ICommandValidator commandValidator, IStudentRepository studentRepository, IMediator mediator)
        {
            _commandValidator = commandValidator;
            _studentRepository = studentRepository;
            _mediator = mediator;
        }

        public async Task<bool> Handle(AddEnrollmentCommand request, CancellationToken cancellationToken)
        {
            if (_commandValidator.Validate(request) == false)
                return false;

            var student = _studentRepository.GetById(request.StudentId);
            if (student == null)
            {
                await _mediator.Publish(new DomainNotification(request.MessageType, "Student not found."), cancellationToken);
                return false;
            }

            var existingEnrollment = _studentRepository.GetEnrollmentByCourseIdAndStudentId(request.CourseId, request.StudentId);
            if (existingEnrollment != null)
            {
                await _mediator.Publish(new DomainNotification(request.MessageType, "Enrollment already exists."), cancellationToken);
                return false;
            }

            var enrollment = EnrollmentPendingPayment.Create(request.StudentId, request.CourseId);
            await _studentRepository.Add(enrollment);

            enrollment.AddEvent(new EnrollmentAddedEvent(enrollment.Id, enrollment.StudentId, enrollment.CourseId));
            return await _studentRepository.UnitOfWork.Commit();
        }

        public async Task<bool> Handle(EnrollmentPaidCommand request, CancellationToken cancellationToken)
        {
            if (_commandValidator.Validate(request) == false)
                return false;

            var existingEnrollment = await _studentRepository.GetEnrollmentsById(request.EnrollmentId);
            if (existingEnrollment == null)
            {
                await _mediator.Publish(new DomainNotification(request.MessageType, "Enrollment not found."), cancellationToken);
                return false;
            }


            existingEnrollment.Activate();
            await _studentRepository.Update(existingEnrollment);

            return await _studentRepository.UnitOfWork.Commit();
        }

        public async Task<bool> Handle(FinishEnrollmentCommand request, CancellationToken cancellationToken)
        {
            var enrollment = await _studentRepository.GetEnrollmentByCourseIdAndStudentId(request.CourseId, request.StudentId);
            if (enrollment == null)
            {
                await _mediator.Publish(new DomainNotification(request.MessageType, "Enrollment not found."), cancellationToken);
                return false;
            }

            if (enrollment.Status != EnrollmentStatus.Active)
            {
                await _mediator.Publish(new DomainNotification(request.MessageType, "Enrollment is not active."), cancellationToken);
                return false;
            }

            enrollment.Close();
            enrollment.AddEvent(new EnrollmentFinishedEvent(request.StudentId, request.CourseId, enrollment.Id));
            await _studentRepository.Update(enrollment);

            return await _studentRepository.UnitOfWork.Commit();
        }
    }
}
