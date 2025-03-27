using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using TheatricalPlayersRefactoringKata.Application.DTOs.Response;
using TheatricalPlayersRefactoringKata.Domain.Repositories;

namespace TheatricalPlayersRefactoringKata.Application.UseCases.Play.GetById
{
    public class CommandHandler : IRequestHandler<Command, PlayResponse>
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

        public async Task<PlayResponse> Handle(Command request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{nameof(Handle)}");
            var play = await _repository.GetByIdAsync(request.id);
            if (play is null)
                throw new KeyNotFoundException("Play not found");

            var result = _mapper.Map<PlayResponse>(play);
            return result;
        }
    }
}