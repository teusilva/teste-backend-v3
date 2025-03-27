using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using TheatricalPlayersRefactoringKata.Application.DTOs.Response;
using TheatricalPlayersRefactoringKata.Application.UseCases.Performance.GetById;
using TheatricalPlayersRefactoringKata.Domain.Repositories;

namespace TheatricalPlayersRefactoringKata.Application.Tests.UseCases.Performance.GetById
{
    public class CommandHandlerTests
    {
        private readonly Mock<ILogger<CommandHandler>> _loggerMock;
        private readonly Mock<IPerformanceRepository> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly CommandHandler _handler;

        public CommandHandlerTests()
        {
            _loggerMock = new Mock<ILogger<CommandHandler>>();
            _repositoryMock = new Mock<IPerformanceRepository>();
            _mapperMock = new Mock<IMapper>();

            _handler = new CommandHandler(
                _loggerMock.Object,
                _repositoryMock.Object,
                _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnPerformanceResponse_WhenPerformanceExists()
        {
            // Arrange
            var PerformanceId = Guid.NewGuid();
            var Performance = new Domain.Entities.Performance { Id = PerformanceId, Audience = 1 };
            var PerformanceResponse = new PerformanceResponse { Id = PerformanceId, Audience = 1 };

            var command = new Command(PerformanceId);

            _repositoryMock.Setup(r => r.GetByIdAsync(PerformanceId))
                .ReturnsAsync(Performance);

            _mapperMock.Setup(m => m.Map<PerformanceResponse>(Performance))
                .Returns(PerformanceResponse);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(PerformanceResponse.Id, result.Id);
            Assert.Equal(PerformanceResponse.Audience, result.Audience);

        }

        [Fact]
        public async Task Handle_ShouldThrowKeyNotFoundException_WhenPerformanceDoesNotExist()
        {
            // Arrange
            var PerformanceId = Guid.NewGuid();
            var command = new Command(PerformanceId);

            _repositoryMock.Setup(r => r.GetByIdAsync(PerformanceId))
                .ReturnsAsync((Domain.Entities.Performance)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        }
    }
}
