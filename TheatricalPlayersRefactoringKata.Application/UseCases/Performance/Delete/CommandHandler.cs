using MediatR;
using Microsoft.Extensions.Logging;
using TheatricalPlayersRefactoringKata.Domain.Repositories;

namespace TheatricalPlayersRefactoringKata.Application.UseCases.Performance.Delete
{
    public class CommandHandler : IRequestHandler<Command, Guid>
    {
        private readonly ILogger<CommandHandler> _logger;
        private readonly IPerformanceRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        public CommandHandler(ILogger<CommandHandler> logger,
            IPerformanceRepository repository,
            IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _repository = repository;
            _unitOfWork = unitOfWork;
        }


        public async Task<Guid> Handle(Command command, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{nameof(Handle)}.{command.id}");
            var performance = await _repository.GetByIdAsync(command.id);
            if (performance is null)
                throw new KeyNotFoundException("Performance not found");

            await _repository.DeleteAsync(performance);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return performance.Id;
        }
    }
}