using MediatR;
using Microsoft.Extensions.Logging;
using TheatricalPlayersRefactoringKata.Application.ExternalServices.RabbitMQ.Publisher;

namespace TheatricalPlayersRefactoringKata.Application.UseCases.Invoice.InvoiceProcess
{
    public class CommandHandler : IRequestHandler<Command, Guid>
    {
        private readonly ILogger<CommandHandler> _logger;
        private readonly IInvoicePublisher _invoicePublisher;
        public CommandHandler(ILogger<CommandHandler> logger,
            IInvoicePublisher invoicePublisher)
        {
            _logger = logger;
            _invoicePublisher = invoicePublisher;
        }

        public async Task<Guid> Handle(Command command, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{nameof(Handle)}");
            _invoicePublisher.Publish(command.id);
            return command.id;
        }
    }
}