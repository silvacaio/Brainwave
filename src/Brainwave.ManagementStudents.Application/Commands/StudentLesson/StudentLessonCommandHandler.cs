using Brainwave.Core.Extensions;
using Brainwave.Core.Messages.CommonMessages.Notifications;
using Brainwave.ManagementStudents.Application.Commands.User;
using Brainwave.ManagementStudents.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Brainwave.ManagementStudents.Domain.StudentLesson;

namespace Brainwave.ManagementStudents.Application.Commands.StudentLesson
{
    public class StudentLessonCommandHandler :
        IRequestHandler<FinishLessonCommand, bool>
    {

        private readonly ICommandValidator _commandValidator;
        private readonly IStudentRepository _studentRepository;
        private readonly IMediator _mediator;

        public StudentLessonCommandHandler(
            ICommandValidator commandValidator,
            IStudentRepository studentRepository,
            IMediator mediator)
        {
            _commandValidator = commandValidator;
            _studentRepository = studentRepository;
            _mediator = mediator;
        }

        public async Task<bool> Handle(FinishLessonCommand request, CancellationToken cancellationToken)
        {
            if (_commandValidator.Validate(request) == false)
                return false;

            var student = _studentRepository.GetById(request.StudentId);
            if (student == null)
            {
                await _mediator.Publish(new DomainNotification(request.MessageType, "Student not found."), cancellationToken);
                return false;
            }

            var lesson = _studentRepository.GetLessonByStudentIdAndCourseIdAndLessonId(request.StudentId, request.CourseId, request.LessonId);
            if(lesson != null)
            {
                await _mediator.Publish(new DomainNotification(request.MessageType, "Student already finish this Lesson."), cancellationToken);
                return false;
            }

            var newLesson = StudentLessonFactory.Create(request.StudentId, request.CourseId, request.LessonId);
            _studentRepository.Add(newLesson);
            return await _studentRepository.UnitOfWork.Commit();    

        }
    }
}
