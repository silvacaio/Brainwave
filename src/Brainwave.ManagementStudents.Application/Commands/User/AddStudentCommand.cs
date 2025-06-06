using Brainwave.Core.Messages;
using FluentValidation;

namespace Brainwave.ManagementStudents.Application.Commands.User
{
    public class AddStudentCommand : Command
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }

        public AddStudentCommand(Guid userId, string name)
        {
            UserId = userId;
            Name = name;
        }

        public override bool IsValid()
        {
            ValidationResult = new AddStudentCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class AddStudentCommandValidation : AbstractValidator<AddStudentCommand>
    {
        public AddStudentCommandValidation()
        {
            RuleFor(c => c.UserId)
                .NotEmpty()
                .WithMessage("The UserId field is required.");

            RuleFor(c => c.Name)
                .NotEmpty()
                .WithMessage("The name field is required.");
        }
    }

}
