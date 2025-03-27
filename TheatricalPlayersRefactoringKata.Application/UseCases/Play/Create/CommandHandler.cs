using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using TheatricalPlayersRefactoringKata.Domain.Repositories;

namespace TheatricalPlayersRefactoringKata.Application.UseCases.Play.Create
{
    public class CommandHandler : IRequestHandler<Command, Guid>
    {
        private readonly ILogger<CommandHandler> _logger;
        private readonly IPlayRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CommandHandler(ILogger<CommandHandler> logger,
            IPlayRepository repository,
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
            var play = _mapper.Map<Domain.Entities.Play>(command.request);
            await _repository.AddAsync(play);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return play.Id;
        }
    }
}