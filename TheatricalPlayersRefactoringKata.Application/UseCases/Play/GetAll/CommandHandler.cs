using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TheatricalPlayersRefactoringKata.Application.DTOs.Response;
using TheatricalPlayersRefactoringKata.Domain.Repositories;

namespace TheatricalPlayersRefactoringKata.Application.UseCases.Play.GetAll
{
    public class CommandHandler : IRequestHandler<Command, List<PlayResponse>>
    {
        private readonly ILogger<CommandHandler> _logger;
        private readonly IPlayRepository _repository;
        private readonly IMapper _mapper;
        public CommandHandler(ILogger<CommandHandler> logger,
            IPlayRepository repository,
            IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<PlayResponse>> Handle(Command request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{nameof(Handle)}");
           
            var listPlay = await _repository.Query()
                .Skip((request.PageOptions.Page - 1) * request.PageOptions.PageSize)
                .Take(request.PageOptions.PageSize)
                .OrderBy(x => x.CreatedAt)
                .ToListAsync();

            var result = listPlay.Select(a => new PlayResponse
            {
                
            }).ToList();
            return result;
        }
    }
}