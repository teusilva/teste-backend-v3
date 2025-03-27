using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using TheatricalPlayersRefactoringKata.Application.DTOs.Request;
using TheatricalPlayersRefactoringKata.Application.UseCases.Performance.Create;
using TheatricalPlayersRefactoringKata.Domain.Repositories;

namespace TheatricalPlayersRefactoringKata.Application.Tests.UseCases.Performance.Create
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
        public async Task Handle_ShouldCreatePerformanceAndReturnId()
        {
            // Arrange
            var PerformanceId = Guid.NewGuid();
            var request = new PerformanceRequest { Audience = 1 , PlayId = Guid.NewGuid() };
            var command = new Command(request);

            var PerformanceEntity = new Domain.Entities.Performance { Audience = 1, PlayId = Guid.NewGuid(), Id = PerformanceId };

            _mapperMock.Setup(m => m.Map<Domain.Entities.Performance>(command.request))
                       .Returns(PerformanceEntity);

            _repositoryMock.Setup(r => r.AddAsync(It.IsAny<Domain.Entities.Performance>()))
                           .ReturnsAsync(It.IsAny<Domain.Entities.Performance>());

            _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
                           .Returns(Task.CompletedTask);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.Equal(PerformanceId, result);
        }
    }
}
