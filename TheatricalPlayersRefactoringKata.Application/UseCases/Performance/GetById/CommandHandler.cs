using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using TheatricalPlayersRefactoringKata.Application.DTOs.Response;
using TheatricalPlayersRefactoringKata.Domain.Repositories;

namespace TheatricalPlayersRefactoringKata.Application.UseCases.Performance.GetById
{
    public class CommandHandler : IRequestHandler<Command, PerformanceResponse>
    {
        private readonly ILogger<CommandHandler> _logger;
        private readonly IPerformanceRepository _repository;
        private readonly IMapper _mapper;
        public CommandHandler(ILogger<CommandHandler> logger,
            IPerformanceRepository repository,
            IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PerformanceResponse> Handle(Command request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{nameof(Handle)}");
            var performance = await _repository.GetByIdAsync(request.id);
            if (performance is null)
                throw new KeyNotFoundException("Performance not found");

            var result = _mapper.Map<PerformanceResponse> (performance);
            return result;
        }
    }
}