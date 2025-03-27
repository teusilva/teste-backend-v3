using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TheatricalPlayersRefactoringKata.Application.DTOs.Response;
using TheatricalPlayersRefactoringKata.Domain.Repositories;

namespace TheatricalPlayersRefactoringKata.Application.UseCases.Invoice.GetAll
{
    public class CommandHandler : IRequestHandler<Command, List<InvoiceResponse>>
    {
        private readonly ILogger<CommandHandler> _logger;
        private readonly IInvoiceRepository _repository;
        public CommandHandler(ILogger<CommandHandler> logger,
            IInvoiceRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public async Task<List<InvoiceResponse>> Handle(Command request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{nameof(Handle)}");
           
            var listInvoice = await _repository.Query()
                .Skip((request.PageOptions.Page - 1) * request.PageOptions.PageSize)
                .Take(request.PageOptions.PageSize)
                .OrderBy(x => x.CreatedAt)
                .ToListAsync();

            var result = listInvoice.Select(a => new InvoiceResponse
            {
                Customer = a.Customer,
                Id = a.Id,
            }).ToList();
            return result;
        }
    }
}