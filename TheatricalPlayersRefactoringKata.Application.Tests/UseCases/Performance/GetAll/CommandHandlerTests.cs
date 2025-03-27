using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;
using TheatricalPlayersRefactoringKata.Application.DTOs.Request;
using TheatricalPlayersRefactoringKata.Application.UseCases.Performance.GetAll;
using TheatricalPlayersRefactoringKata.Domain.Repositories;
using MockQueryable.Moq;
using MockQueryable;

namespace TheatricalPlayersRefactoringKata.Application.Tests.UseCases.Performance.GetAll
{
    public class CommandHandlerTests
    {
        private readonly Mock<ILogger<CommandHandler>> _loggerMock;
        private readonly Mock<IPerformanceRepository> _repositoryMock;
        private readonly CommandHandler _handler;

        public CommandHandlerTests()
        {
            _loggerMock = new Mock<ILogger<CommandHandler>>();
            _repositoryMock = new Mock<IPerformanceRepository>();

            _handler = new CommandHandler(
                _loggerMock.Object,
                _repositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnPerformances_WhenPerformancesExist()
        {
            // Arrange
            var Performances = new List<Domain.Entities.Performance>
            {
                new Domain.Entities.Performance { Id = Guid.NewGuid(), Audience = 1 , CreatedAt = DateTime.UtcNow },
                new Domain.Entities.Performance { Id = Guid.NewGuid(), Audience = 1, CreatedAt = DateTime.UtcNow.AddMinutes(-10) }
            };

            var pageOptions = new PageOptions { Page = 1, PageSize = 10 };
            var command = new Command(pageOptions);

            var mockQueryable = Performances.AsQueryable().BuildMock();

            _repositoryMock.Setup(r => r.Query()).Returns(mockQueryable);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }
    }
}
