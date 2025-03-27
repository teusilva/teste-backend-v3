using MediatR;
using Microsoft.Extensions.Logging;
using TheatricalPlayersRefactoringKata.Domain.Repositories;

namespace TheatricalPlayersRefactoringKata.Application.UseCases.Invoice.Delete
{
    public class CommandHandler : IRequestHandler<Command, Guid>
    {
        private readonly ILogger<CommandHandler> _logger;
        private readonly IInvoiceRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        public CommandHandler(ILogger<CommandHandler> logger,
            IInvoiceRepository repository,
            IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(Command request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{nameof(Handle)}.{request.id}");
            var invoice = await _repository.GetByIdAsync(request.id);
            if (invoice is null)
                throw new KeyNotFoundException("Invoice not found");

            await _repository.DeleteAsync(invoice);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return invoice.Id;
        }
    }
}