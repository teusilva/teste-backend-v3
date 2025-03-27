using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using TheatricalPlayersRefactoringKata.Domain.Repositories;

namespace TheatricalPlayersRefactoringKata.Application.UseCases.Invoice.Create
{
    public class CommandHandler : IRequestHandler<Command, Guid>
    {
        private readonly ILogger<CommandHandler> _logger;
        private readonly IInvoiceRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CommandHandler(ILogger<CommandHandler> logger,
            IInvoiceRepository repository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(Command command, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{nameof(Handle)}");
            var invoice = _mapper.Map<Domain.Entities.Invoice>(command.request);
            await _repository.AddAsync(invoice);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return invoice.Id;
        }
    }
}