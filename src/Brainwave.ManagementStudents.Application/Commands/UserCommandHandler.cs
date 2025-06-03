using Brainwave.Core.Extensions;
using Brainwave.ManagementStudents.Domain;
using MediatR;
using static Brainwave.ManagementStudents.Domain.Student;

namespace Brainwave.ManagementStudents.Application.Commands
{
    public class UserCommandHandler :
        IRequestHandler<AddStudentCommand, bool>,
        IRequestHandler<AddAdminCommand, bool>

    {
        private readonly ICommandValidator _commandValidator;
        private readonly IStudentRepository _studentRepository;
        private readonly IMediator _mediator;

        public UserCommandHandler(ICommandValidator commandValidator, IStudentRepository studentRepository, IMediator mediator)
        {
            _commandValidator = commandValidator;
            _studentRepository = studentRepository;
            _mediator = mediator;
        }

        public async Task<bool> Handle(AddStudentCommand request, CancellationToken cancellationToken)
        {
            if (_commandValidator.Validate(request) == false)
                return false;

            var student = StudentFactory.CreateStudent(request.UserId, request.Name);

            await _studentRepository.Add(student);
            return await _studentRepository.UnitOfWork.Commit();
        }

        public async Task<bool> Handle(AddAdminCommand request, CancellationToken cancellationToken)
        {
            if (_commandValidator.Validate(request) == false)
                return false;

            var student = StudentFactory.CreateAdmin(request.UserId, request.Name);

            await _studentRepository.Add(student);
            return await _studentRepository.UnitOfWork.Commit();
        }
    }
}
