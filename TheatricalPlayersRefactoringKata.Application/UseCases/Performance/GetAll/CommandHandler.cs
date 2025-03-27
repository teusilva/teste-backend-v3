using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TheatricalPlayersRefactoringKata.Application.DTOs.Response;
using TheatricalPlayersRefactoringKata.Domain.Repositories;

namespace TheatricalPlayersRefactoringKata.Application.UseCases.Performance.GetAll
{
    public class CommandHandler : IRequestHandler<Command, List<PerformanceResponse>>
    {
        private readonly ILogger<CommandHandler> _logger;
        private readonly IPerformanceRepository _repository;
        public CommandHandler(ILogger<CommandHandler> logger,
            IPerformanceRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public async Task<List<PerformanceResponse>> Handle(Command request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{nameof(Handle)}");
           
            var listPerformance = await _repository.Query()
                .Skip((request.PageOptions.Page - 1) * request.PageOptions.PageSize)
                .Take(request.PageOptions.PageSize)
                .OrderBy(x => x.CreatedAt)
                .ToListAsync();

            var result = listPerformance.Select(a => new PerformanceResponse
            {
                Id = a.Id,
                Audience = a.Audience,
                PlayId = a.PlayId
            }).ToList();
            return result;
        }
    }
}