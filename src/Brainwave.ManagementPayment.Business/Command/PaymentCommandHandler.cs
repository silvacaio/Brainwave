using Brainwave.Core.Communication.Mediator;
using Brainwave.Core.Messages.CommonMessages.Notifications;
using Brainwave.Core.Messages.IntegrationEvents;
using Brainwave.ManagementPayment.Application.Commands;
using Brainwave.ManagementPayment.Business;
using Brainwave.ManagementPayment.Business.ValueObjects;
using MediatR;

namespace Brainwave.ManagementPayment.Application.Commands
{
    public class PaymentCommandHandler :
          IRequestHandler<MakePaymentCommand, bool>
    {

        private readonly ICreditCardPaymentFacade _creditCardPaymentFacade;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IMediator _mediator;

        public PaymentCommandHandler(ICreditCardPaymentFacade creditCardPaymentFacade,
            IPaymentRepository paymentRepository,
           IMediator mediator)
        {
            _creditCardPaymentFacade = creditCardPaymentFacade;
            _paymentRepository = paymentRepository;
            _mediator = mediator;
        }

        public async Task<bool> Handle(MakePaymentCommand request, CancellationToken cancellationToken)
        {
            var creditCart = new CreditCard(request.CardNumber, request.CardHolderName, request.ExpirationDate, request.SecurityCode);

            var payment = new Payment(request.EnrollmentId, request.Value, creditCart);

            var transaction = _creditCardPaymentFacade.ProcessPayment(payment);

            if (transaction.StatusTransaction == StatusTransaction.Refused)
            {
                await _mediator.Publish(new DomainNotification(request.MessageType, "The payment was declined by the payment processor."), cancellationToken);
                return false;
            }

            payment.AddEvent(new EnrollmentPaidEvent(request.UserId, transaction.EnrollmentId, transaction.PaymentId, transaction.Value));

            _paymentRepository.Add(payment);
            _paymentRepository.AddTransaction(transaction);

            return await _paymentRepository.UnitOfWork.Commit();

        }
    }
}
