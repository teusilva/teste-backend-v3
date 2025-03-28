using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using TheatricalPlayersRefactoringKata.Application.DTOs.Request;
using TheatricalPlayersRefactoringKata.Application.DTOs.Response;
using TheatricalPlayersRefactoringKata.Domain.Entities;
using TheatricalPlayersRefactoringKata.Domain.Enums;
using TheatricalPlayersRefactoringKata.WebApi.Controllers;

namespace TheatricalPlayersRefactoringKata.WebApi.Tests
{
    public class PlayControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<ILogger<PlayController>> _loggerMock;
        private readonly PlayController _controller;

        public PlayControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _loggerMock = new Mock<ILogger<PlayController>>();
            _controller = new PlayController(_mediatorMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task PlayCreateAsync_ReturnsCreatedResult_WhenPlayIsCreatedSuccessfully()
        {
            var id = Guid.NewGuid();
            var request = new PlayRequest
            {
                Name = "Hamlet",
                Type = Enum.Parse<TypesPlays>("Tragedy"),
                Lines = 4000,
            };

            var playResponse = new PlayResponse
            {
                PerformanceId = Guid.NewGuid(),
                Name = "Hamlet",
                Lines = 4000,
                Type = Enum.Parse<TypesPlays>("Tragedy"),
                Performance = new Performance
                {
                    Id = Guid.NewGuid(),
                    PlayId = id,
                    Audience = 30,
                }
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<Application.UseCases.Play.Create.Command>(), default))
                .ReturnsAsync(id);

            var result = await _controller.PlayCreateAsync(request);

            var createdResult = Assert.IsType<CreatedResult>(result);
            Assert.Equal(201, createdResult.StatusCode);
            Assert.Equal(id, createdResult.Value);
        }

        [Fact]
        public async Task PlayUpdateAsync_ReturnsOkResult_WhenPlayIsUpdatedSuccessfully()
        {
            var id = Guid.NewGuid();
            var request = new PlayRequest
            {
                Name = "Macbeth",
                Type = Enum.Parse<TypesPlays>("Tragedy"),
                Lines = 2000

            };

            var playResponse = new PlayResponse
            {
                PerformanceId = Guid.NewGuid(),
                Name = "Macbeth",
                Type = Enum.Parse<TypesPlays>("Tragedy"),
                Lines = 2000,
                Performance = new Performance
                {
                    Id = Guid.NewGuid(),
                    Audience = 30,
                }

            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<Application.UseCases.Play.Update.Command>(), default))
                .ReturnsAsync(id);

            var result = await _controller.PlayUpdateAsync(id, request);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(id, okResult.Value);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsOkResult_WithListOfPlays()
        {
            var pageOptions = new PageOptions { Page = 1, PageSize = 10 };
            var playList = new List<PlayResponse>
            {
                new PlayResponse {Name = "Hamlet", Type = Enum.Parse<TypesPlays>("Tragedy")},
                new PlayResponse {Name = "Othello", Type = Enum.Parse<TypesPlays>("Tragedy")}
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<Application.UseCases.Play.GetAll.Command>(), default))
                .ReturnsAsync(playList);

            var result = await _controller.GetAllAsync(pageOptions);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(playList, okResult.Value);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsOkResult_WhenPlayIsFound()
        {
            var id = Guid.NewGuid();
            var playResponse = new PlayResponse
            {
                Name = "King Lear",
                Type = Enum.Parse<TypesPlays>("Tragedy")
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<Application.UseCases.Play.GetById.Command>(), default))
                .ReturnsAsync(playResponse);

            var result = await _controller.GetByIdAsync(id);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(playResponse, okResult.Value);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNotFound_WhenPlayDoesNotExist()
        {
            var id = Guid.NewGuid();

            _mediatorMock.Setup(m => m.Send(It.IsAny<Application.UseCases.Play.GetById.Command>(), default))
                .ReturnsAsync((PlayResponse)null);

            var result = await _controller.GetByIdAsync(id);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsOkResult_WhenPlayIsDeletedSuccessfully()
        {
            var id = Guid.NewGuid();
            var deleteResponse = new { Success = true };

            _mediatorMock.Setup(m => m.Send(It.IsAny<Application.UseCases.Play.Delete.Command>(), default))
                .ReturnsAsync(id);

            var result = await _controller.DeleteAsync(id);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(id, okResult.Value);
        }
    }
}
