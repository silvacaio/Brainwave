using Brainwave.Core.Messages;
using FluentValidation;


namespace Brainwave.ManagementCourses.Application.Commands
{
    public class AddLessonToCourseCommand : Command
    {
        public AddLessonToCourseCommand(string title, string content, string material, Guid courseId)
        {
            Title = title;
            Content = content;
            Material = material;
            CourseId = courseId;
        }

        public string Title { get; private set; }
        public string Content { get; set; }
        public string Material { get; set; }
        public Guid CourseId { get; set; }

        public override bool IsValid()
        {
            ValidationResult = new AddLessonToCourseCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class AddLessonToCourseCommandValidation : AbstractValidator<AddLessonToCourseCommand>
    {
        public AddLessonToCourseCommandValidation()
        {
            RuleFor(c => c.Title)
                .NotEmpty()
                .WithMessage("Title is required");

            RuleFor(c => c.Content)
                .NotEmpty()
                .WithMessage("Content Content is required");

            RuleFor(c => c.Material)
         .NotEmpty()
         .WithMessage("Material languague is required");

            RuleFor(c => c.CourseId)
           .NotEqual(Guid.Empty)
           .WithMessage("Invalid course");
        }
    }
}
