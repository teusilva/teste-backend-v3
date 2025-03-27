using MediatR;
using Microsoft.Extensions.Logging;
using TheatricalPlayersRefactoringKata.Domain.Repositories;

namespace TheatricalPlayersRefactoringKata.Application.UseCases.Play.Delete
{
    public class CommandHandler : IRequestHandler<Command, Guid>
    {
        private readonly ILogger<CommandHandler> _logger;
        private readonly IPlayRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        public CommandHandler(ILogger<CommandHandler> logger,
            IPlayRepository repository,
            IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _repository = repository;
            _unitOfWork = unitOfWork;
        }


        public async Task<Guid> Handle(Command request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{nameof(Handle)}.{request.id}");
            var play = await _repository.GetByIdAsync(request.id);
            if (play is null)
                throw new KeyNotFoundException("Play not found");

            await _repository.DeleteAsync(play);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return play.Id;
        }
    }
}