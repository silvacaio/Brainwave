using Brainwave.ManagementStudents.Domain;
using MediatR;

namespace Brainwave.ManagementStudents.Application.Commands.Cetificates
{
    public class CertificateCommandHandler :
        IRequestHandler<CreateCertificateCommand, bool>
    {
        public Task<bool> Handle(CreateCertificateCommand request, CancellationToken cancellationToken)
        {
            //TODO FAZER AINDA
            //var certificate = new Certificate(
            return Task.FromResult(true);
        }
    }
}
