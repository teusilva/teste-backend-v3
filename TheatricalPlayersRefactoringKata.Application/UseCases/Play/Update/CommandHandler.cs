using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using TheatricalPlayersRefactoringKata.Domain.Repositories;

namespace TheatricalPlayersRefactoringKata.Application.UseCases.Play.Update
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


        public async Task<Guid> Handle(Command request, CancellationToken cancellationToken)
        {
            var play = await _repository.GetByIdAsync(request.Id);
            if (play is null)
                throw new KeyNotFoundException("Play not found");

            _mapper.Map(request.request, play);

            await _repository.UpdateAsync(play);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return play.Id;
        }
    }
}