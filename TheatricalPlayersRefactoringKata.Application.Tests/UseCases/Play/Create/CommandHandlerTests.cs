using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using TheatricalPlayersRefactoringKata.Application.DTOs.Request;
using TheatricalPlayersRefactoringKata.Application.UseCases.Play.Create;
using TheatricalPlayersRefactoringKata.Domain.Repositories;

namespace TheatricalPlayersRefactoringKata.Application.Tests.UseCases.Play.Create
{
    public class CommandHandlerTests
    {
        private readonly Mock<ILogger<CommandHandler>> _loggerMock;
        private readonly Mock<IPlayRepository> _repositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly CommandHandler _handler;

        public CommandHandlerTests()
        {
            _loggerMock = new Mock<ILogger<CommandHandler>>();
            _repositoryMock = new Mock<IPlayRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();

            _handler = new CommandHandler(
                _loggerMock.Object,
                _repositoryMock.Object,
                _unitOfWorkMock.Object,
                _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldCreatePlayAndReturnId()
        {
            // Arrange
            var PlayId = Guid.NewGuid();
            var request = new PlayRequest { Type = Domain.Enums.TypesPlays.Tragedy, Lines = 1 , Name = "teste" };
            var command = new Command(request);

            var PlayEntity = new Domain.Entities.Play { Id = PlayId ,Type = Domain.Enums.TypesPlays.Tragedy, Lines = 1, Name = "teste" };

            _mapperMock.Setup(m => m.Map<Domain.Entities.Play>(command.request))
                       .Returns(PlayEntity);

            _repositoryMock.Setup(r => r.AddAsync(It.IsAny<Domain.Entities.Play>()))
                           .ReturnsAsync(It.IsAny<Domain.Entities.Play>());

            _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
                           .Returns(Task.CompletedTask);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.Equal(PlayId, result);
        }
    }
}
