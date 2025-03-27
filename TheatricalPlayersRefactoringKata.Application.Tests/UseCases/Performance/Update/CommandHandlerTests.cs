using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using TheatricalPlayersRefactoringKata.Application.DTOs.Request;
using TheatricalPlayersRefactoringKata.Application.UseCases.Performance.Update;
using TheatricalPlayersRefactoringKata.Domain.Repositories;

namespace TheatricalPlayersRefactoringKata.Application.Tests.UseCases.Performance.Update
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
            _mapperMock = new Mock<IMapper>();

            _handler = new CommandHandler(
                _loggerMock.Object,
                _repositoryMock.Object,
                _unitOfWorkMock.Object,
                _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnPerformanceId_WhenPerformanceExists()
        {
            var PerformanceId = Guid.NewGuid();
            var Performance = new Domain.Entities.Performance { Id = PerformanceId, Audience = 1 };

            var request = new PerformanceRequest { Audience = 1 };
            var command = new Command(PerformanceId, request);

            _repositoryMock.Setup(r => r.GetByIdAsync(PerformanceId))
                .ReturnsAsync(Performance);

            _mapperMock.Setup(m => m.Map(request, Performance));

            _repositoryMock.Setup(r => r.UpdateAsync(Performance))
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
            var request = new PerformanceRequest { Audience = 1,PlayId = Guid.NewGuid() };
            var command = new Command(PerformanceId, request);

            _repositoryMock.Setup(r => r.GetByIdAsync(PerformanceId))
                .ReturnsAsync((Domain.Entities.Performance)null);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        }
    }
}