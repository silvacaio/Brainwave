using Brainwave.Core.Messages;
using FluentValidation;

namespace Brainwave.ManagementStudents.Application.Commands.StudentLesson
{
    public class FinishLessonCommand(Guid studentId, Guid courseId, Guid lessonId) : Command
    {
        public Guid StudentId { get; set; } = studentId;
        public Guid CourseId { get; set; } = courseId;
        public Guid LessonId { get; set; } = lessonId;

        public override bool IsValid()
        {
            ValidationResult = new FinishLessonCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class FinishLessonCommandValidation : AbstractValidator<FinishLessonCommand>
    {
        public FinishLessonCommandValidation()
        {
            RuleFor(c => c.StudentId)
                .NotEqual(Guid.Empty)
                .WithMessage("The StudentId field cannot be empty.");

            RuleFor(c => c.CourseId)
                .NotEqual(Guid.Empty)
                .WithMessage("The CourseId field cannot be empty.");

            RuleFor(c => c.LessonId)
              .NotEqual(Guid.Empty)
              .WithMessage("The LessonId field cannot be empty.");
        }
    }
}
