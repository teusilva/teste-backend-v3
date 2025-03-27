using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using TheatricalPlayersRefactoringKata.Application.UseCases.Performance.Delete;
using TheatricalPlayersRefactoringKata.Domain.Repositories;

namespace TheatricalPlayersRefactoringKata.Application.Tests.UseCases.Performance.Delete
{
    public class CommandHandlerTests
    {
        private readonly Mock<ILogger<CommandHandler>> _loggerMock;
        private readonly Mock<IPerformanceRepository> _repositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly CommandHandler _handler;

        public CommandHandlerTests()
        {
            _loggerMock = new Mock<ILogger<CommandHandler>>();
            _repositoryMock = new Mock<IPerformanceRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            _handler = new CommandHandler(
                _loggerMock.Object,
                _repositoryMock.Object,
                _unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldDeletePerformanceAndReturnId_WhenPerformanceExists()
        {
            var PerformanceId = Guid.NewGuid();
            var PerformanceEntity = new Domain.Entities.Performance { Id = PerformanceId, Audience = 1 };
            var command = new Command(PerformanceId);

            _repositoryMock.Setup(r => r.GetByIdAsync(PerformanceId))
                           .ReturnsAsync(PerformanceEntity);

            _repositoryMock.Setup(r => r.DeleteAsync(It.IsAny<Domain.Entities.Performance>()))
                           .Returns(Task.CompletedTask);

            _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
                           .Returns(Task.CompletedTask);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.Equal(PerformanceId, result);
           
        }

        [Fact]
        public async Task Handle_ShouldThrowKeyNotFoundException_WhenPerformanceDoesNotExist()
        {
            var PerformanceId = Guid.NewGuid();
            var command = new Command(PerformanceId);

            _repositoryMock.Setup(r => r.GetByIdAsync(PerformanceId))
                           .ReturnsAsync(value: null);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        }
    }
}
