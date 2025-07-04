using Brainwave.API.Controllers.Base;
using Brainwave.Core.Messages.CommonMessages.Notifications;
using Brainwave.ManagementStudents.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Brainwave.API.Controllers
{
    [Route("api/students")]

    public class StudentsController : MainController
    {
        private readonly IStudentQueries _studentQueries;
        private readonly IMediator _mediator;

        public StudentsController(INotificationHandler<DomainNotification> notifications,
            IMediator mediator,
            IStudentQueries studentQueries) : base(notifications, mediator)
        {
            _studentQueries = studentQueries;
            _mediator = mediator;
        }      
    }
}
