using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using TheatricalPlayersRefactoringKata.Application.DTOs.Request;
using TheatricalPlayersRefactoringKata.Application.UseCases.Play.Update;
using TheatricalPlayersRefactoringKata.Domain.Repositories;

namespace TheatricalPlayersRefactoringKata.Application.Tests.UseCases.Play.Update
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
        public async Task Handle_ShouldReturnPlayId_WhenPlayExists()
        {
            var PlayId = Guid.NewGuid();
            var Play = new Domain.Entities.Play { Id = PlayId, Type = Domain.Enums.TypesPlays.Tragedy, Lines = 1, Name = "teste"     };

            var request = new PlayRequest { Type = Domain.Enums.TypesPlays.Tragedy, Lines = 1, Name = "teste" };
            var command = new Command(PlayId, request);

            _repositoryMock.Setup(r => r.GetByIdAsync(PlayId))
                .ReturnsAsync(Play);

            _mapperMock.Setup(m => m.Map(request, Play));

            _repositoryMock.Setup(r => r.UpdateAsync(Play))
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
            var request = new PlayRequest { Type = Domain.Enums.TypesPlays.Tragedy, Lines = 1, Name = "teste" };
            var command = new Command(PlayId, request);

            _repositoryMock.Setup(r => r.GetByIdAsync(PlayId))
                .ReturnsAsync((Domain.Entities.Play)null);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        }
    }
}