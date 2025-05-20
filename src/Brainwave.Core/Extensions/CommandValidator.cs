using Brainwave.Core.Messages.CommonMessages.Notifications;
using Brainwave.Core.Messages;
using MediatR;

namespace Brainwave.Core.Extensions
{
    public class CommandValidator : ICommandValidator
    {
        private readonly IMediator _mediator;

        public CommandValidator(IMediator mediator)
        {
            _mediator = mediator;
        }
        public bool Validate(Command command)
        {
            if (command.IsValid()) return true;

            foreach (var erro in command.ValidationResult.Errors)
            {
                _mediator.Publish(new DomainNotification(command.MessageType, erro.ErrorMessage));
            }
            return false;
        }
    }
}
