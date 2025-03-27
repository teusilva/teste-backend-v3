using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using TheatricalPlayersRefactoringKata.Application.UseCases.Play.Delete;
using TheatricalPlayersRefactoringKata.Domain.Repositories;

namespace TheatricalPlayersRefactoringKata.Application.Tests.UseCases.Play.Delete
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

            _handler = new CommandHandler(
                _loggerMock.Object,
                _repositoryMock.Object,
                _unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldDeletePlayAndReturnId_WhenPlayExists()
        {
            var PlayId = Guid.NewGuid();
            var PlayEntity = new Domain.Entities.Play { Type = Domain.Enums.TypesPlays.Tragedy, Lines = 1, Name = "teste", Id = PlayId };
            var command = new Command(PlayId);

            _repositoryMock.Setup(r => r.GetByIdAsync(PlayId))
                           .ReturnsAsync(PlayEntity);

            _repositoryMock.Setup(r => r.DeleteAsync(It.IsAny<Domain.Entities.Play>()))
                           .Returns(Task.CompletedTask);

            _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
                           .Returns(Task.CompletedTask);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.Equal(PlayId, result);
           
        }

        [Fact]
        public async Task Handle_ShouldThrowKeyNotFoundException_WhenPlayDoesNotExist()
        {
            var PlayId = Guid.NewGuid();
            var command = new Command(PlayId);

            _repositoryMock.Setup(r => r.GetByIdAsync(PlayId))
                           .ReturnsAsync(value: null);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        }
    }
}
