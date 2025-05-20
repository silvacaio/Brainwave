using Brainwave.Core.Messages;

namespace Brainwave.Core.Extensions
{
    public interface ICommandValidator
    {
        bool Validate(Command command);
    }
}
