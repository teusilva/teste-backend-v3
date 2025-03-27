using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Reflection.Metadata;
using TheatricalPlayersRefactoringKata.Domain.Repositories;

namespace TheatricalPlayersRefactoringKata.Application.UseCases.Invoice.Update
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
            _logger.LogInformation($"Handdler method {typeof(Handle)}");
            var invoice = await _repository.GetByIdAsync(command.Id);
            if (invoice is null)
                throw new KeyNotFoundException("invoice not found");

            _mapper.Map(command.request, invoice);

            await _repository.UpdateAsync(invoice);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return invoice.Id;
        }
    }
}