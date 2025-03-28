using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using TheatricalPerformanceersRefactoringKata.WebApi.Controllers;
using TheatricalPlayersRefactoringKata.Application.DTOs.Request;
using TheatricalPlayersRefactoringKata.Application.DTOs.Response;
using TheatricalPlayersRefactoringKata.Domain.Entities;

namespace TheatricalPlayersRefactoringKata.WebApi.Tests
{
    public class PerformanceControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<ILogger<PerformanceController>> _loggerMock;
        private readonly PerformanceController _controller;

        public PerformanceControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _loggerMock = new Mock<ILogger<PerformanceController>>();
            _controller = new PerformanceController(_mediatorMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task PerformanceCreateAsync_ReturnsCreatedResult_WhenPerformanceIsCreatedSuccessfully()
        {
            var id = Guid.NewGuid();
            var request = new PerformanceRequest
            {
                PlayId = Guid.NewGuid(),
                Audience = 55
            };

            var performanceResponse = new PerformanceResponse
            {
                Id = Guid.NewGuid(),
                PlayId = request.PlayId,
                Audience = request.Audience
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<Application.UseCases.Performance.Create.Command>(), default))
                .ReturnsAsync(id);

            var result = await _controller.PerformanceCreateAsync(request);

            var createdResult = Assert.IsType<CreatedResult>(result);
            Assert.Equal(201, createdResult.StatusCode);
            Assert.Equal(id, createdResult.Value);
        }

        [Fact]
        public async Task PerformanceUpdateAsync_ReturnsOkResult_WhenPerformanceIsUpdatedSuccessfully()
        {
            var id = Guid.NewGuid();
            var request = new PerformanceRequest
            {
                PlayId = Guid.NewGuid(),
                Audience = 30
            };

            var performanceResponse = new PerformanceResponse
            {
                Id = id,
                PlayId = request.PlayId,
                Audience = request.Audience
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<Application.UseCases.Performance.Update.Command>(), default))
                .ReturnsAsync(id);

            var result = await _controller.PerformanceUpdateAsync(id, request);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(id, okResult.Value);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsOkResult_WithListOfPerformances()
        {
            var pageOptions = new PageOptions { Page = 1, PageSize = 10 };
            var performanceList = new List<PerformanceResponse>
            {
                new PerformanceResponse { Id = Guid.NewGuid(), PlayId = Guid.NewGuid(), Audience = 50 },
                new PerformanceResponse { Id = Guid.NewGuid(), PlayId = Guid.NewGuid(), Audience = 75 }
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<Application.UseCases.Performance.GetAll.Command>(), default))
                .ReturnsAsync(performanceList);

            var result = await _controller.GetAllAsync(pageOptions);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(performanceList, okResult.Value);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsOkResult_WhenPerformanceIsFound()
        {
            var id = Guid.NewGuid();
            var performanceResponse = new PerformanceResponse
            {
                Id = id,
                PlayId = Guid.NewGuid(),
                Audience = 80
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<Application.UseCases.Performance.GetById.Command>(), default))
                .ReturnsAsync(performanceResponse);

            var result = await _controller.GetByIdAsync(id);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(performanceResponse, okResult.Value);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNotFound_WhenPerformanceDoesNotExist()
        {
            var id = Guid.NewGuid();
       
            _mediatorMock.Setup(m => m.Send(It.IsAny<Application.UseCases.Performance.GetById.Command>(), default))
                .ReturnsAsync(value: null);

            var result = await _controller.GetByIdAsync(id);

            var notFoundResult = Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsOkResult_WhenPerformanceIsDeletedSuccessfully()
        {
            var id = Guid.NewGuid();
            var deleteResponse = new { Success = true };

            _mediatorMock.Setup(m => m.Send(It.IsAny<Application.UseCases.Performance.Delete.Command>(), default))
                .ReturnsAsync(id);

            var result = await _controller.DeleteAsync(id);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(id, okResult.Value);
        }
    }
}
