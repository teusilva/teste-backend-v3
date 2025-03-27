using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using TheatricalPlayersRefactoringKata.Application.DTOs.Response;
using TheatricalPlayersRefactoringKata.Domain.Repositories;

namespace TheatricalPlayersRefactoringKata.Application.UseCases.Invoice.GetById
{
    public class CommandHandler : IRequestHandler<Command, InvoiceResponse>
    {
        private readonly ILogger<CommandHandler> _logger;
        private readonly IInvoiceRepository _repository;
        private readonly IMapper _mapper;
        public CommandHandler(ILogger<CommandHandler> logger,
            IInvoiceRepository repository,
            IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<InvoiceResponse> Handle(Command request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{nameof(Handle)}");
            var invoice = await _repository.GetByIdAsync(request.id);
            if (invoice is null)
                throw new KeyNotFoundException("Invoice not found");

            var result = _mapper.Map<InvoiceResponse> (invoice);
            return result;
        }
    }
}