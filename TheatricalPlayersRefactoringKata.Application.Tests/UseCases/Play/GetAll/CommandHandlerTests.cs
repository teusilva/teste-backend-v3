using AutoMapper;
using Microsoft.Extensions.Logging;
using MockQueryable;
using Moq;
using TheatricalPlayersRefactoringKata.Application.DTOs.Request;
using TheatricalPlayersRefactoringKata.Application.UseCases.Play.GetAll;
using TheatricalPlayersRefactoringKata.Domain.Repositories;

namespace TheatricalPlayersRefactoringKata.Application.Tests.UseCases.Play.GetAll
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
        public async Task Handle_ShouldReturnPlays_WhenPlaysExist()
        {
            // Arrange
            var Plays = new List<Domain.Entities.Play>
            {
                new Domain.Entities.Play { Id = Guid.NewGuid(), Type = Domain.Enums.TypesPlays.Tragedy, Lines = 1 , Name = "teste" },
                new Domain.Entities.Play { Id = Guid.NewGuid(), Type = Domain.Enums.TypesPlays.Tragedy, Lines = 1, Name = "teste" }
            };

            var pageOptions = new PageOptions { Page = 1, PageSize = 10 };
            var command = new Command(pageOptions);

            var mockQueryable = Plays.AsQueryable().BuildMock();

            _repositoryMock.Setup(r => r.Query()).Returns(mockQueryable);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }
    }
}
