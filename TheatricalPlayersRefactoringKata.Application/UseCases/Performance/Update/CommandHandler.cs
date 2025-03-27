using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using TheatricalPlayersRefactoringKata.Domain.Repositories;

namespace TheatricalPlayersRefactoringKata.Application.UseCases.Performance.Update
{
    public class CommandHandler : IRequestHandler<Command, Guid>
    {
        private readonly ILogger<CommandHandler> _logger;
        private readonly IPerformanceRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CommandHandler(ILogger<CommandHandler> logger,
            IPerformanceRepository repository,
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
            var performance = await _repository.GetByIdAsync(command.Id);
            if (performance is null)
                throw new KeyNotFoundException("Performance not found");

            _mapper.Map(command.request, performance);

            await _repository.UpdateAsync(performance);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return performance.Id;
        }
    }
}