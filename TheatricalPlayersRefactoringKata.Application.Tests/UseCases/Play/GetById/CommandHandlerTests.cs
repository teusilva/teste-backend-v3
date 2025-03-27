using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using TheatricalPlayersRefactoringKata.Application.DTOs.Response;
using TheatricalPlayersRefactoringKata.Application.UseCases.Play.GetById;
using TheatricalPlayersRefactoringKata.Domain.Repositories;

namespace TheatricalPlayersRefactoringKata.Application.Tests.UseCases.Play.GetById
{
    public class CommandHandlerTests
    {
        private readonly Mock<ILogger<CommandHandler>> _loggerMock;
        private readonly Mock<IPlayRepository> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly CommandHandler _handler;

        public CommandHandlerTests()
        {
            _loggerMock = new Mock<ILogger<CommandHandler>>();
            _repositoryMock = new Mock<IPlayRepository>();
            _mapperMock = new Mock<IMapper>();

            _handler = new CommandHandler(
                _loggerMock.Object,
                _repositoryMock.Object,
                _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnPlayResponse_WhenPlayExists()
        {
            // Arrange
            var PlayId = Guid.NewGuid();
            var Play = new Domain.Entities.Play { Id = PlayId, Type = Domain.Enums.TypesPlays.Tragedy, Lines = 1, Name = "teste" };
            var PlayResponse = new PlayResponse { Id = PlayId, Type = Domain.Enums.TypesPlays.Tragedy, Lines = 1, Name = "teste" };

            var command = new Command(PlayId);

            _repositoryMock.Setup(r => r.GetByIdAsync(PlayId))
                .ReturnsAsync(Play);

            _mapperMock.Setup(m => m.Map<PlayResponse>(Play))
                .Returns(PlayResponse);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(PlayResponse.Id, result.Id);
            Assert.Equal(PlayResponse.Name, result.Name);

        }

        [Fact]
        public async Task Handle_ShouldThrowKeyNotFoundException_WhenPlayDoesNotExist()
        {
            // Arrange
            var PlayId = Guid.NewGuid();
            var command = new Command(PlayId);

            _repositoryMock.Setup(r => r.GetByIdAsync(PlayId))
                .ReturnsAsync((Domain.Entities.Play)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        }
    }
}
