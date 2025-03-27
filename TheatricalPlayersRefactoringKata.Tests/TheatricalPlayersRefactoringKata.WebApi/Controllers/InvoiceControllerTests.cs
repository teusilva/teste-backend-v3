using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using TheatricalInvoiceersRefactoringKata.WebApi.Controllers;
using TheatricalPlayersRefactoringKata.Application.DTOs.Request;
using TheatricalPlayersRefactoringKata.Application.DTOs.Response;

namespace TheatricalPlayersRefactoringKata.WebApi.Tests.Controllers
{
    public class InvoiceControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<ILogger<InvoiceController>> _loggerMock;
        private readonly InvoiceController _controller;

        public InvoiceControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _loggerMock = new Mock<ILogger<InvoiceController>>();
            _controller = new InvoiceController(_mediatorMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task InvoiceCreateAsync_ReturnsCreatedResult_WhenInvoiceIsCreatedSuccessfully()
        {
            // Arrange
            var request = new InvoiceRequest
            {
                Customer = "John Doe",
                PerformancesId = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() }
            };

            var invoiceResponse = new InvoiceResponse
            {
                Id = Guid.NewGuid(),
                Customer = "John Doe",
                PerformancesId = request.PerformancesId,
                TotalAmount = 200.00m
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<InvoiceCreate.Command>(), default))
                .ReturnsAsync(invoiceResponse);

            // Act
            var result = await _controller.InvoiceCreateAsync(request);

            // Assert
            var createdResult = Assert.IsType<CreatedResult>(result);
            Assert.Equal(201, createdResult.StatusCode);
            Assert.Equal(string.Empty, createdResult.Location);
        }

        [Fact]
        public async Task InvoiceUpdateAsync_ReturnsOkResult_WhenInvoiceIsUpdatedSuccessfully()
        {
            // Arrange
            var id = Guid.NewGuid();
            var request = new InvoiceRequest
            {
                Customer = "Jane Doe",
                PerformancesId = new List<Guid> { Guid.NewGuid() }
            };

            var invoiceResponse = new InvoiceResponse
            {
                Id = id,
                Customer = "Jane Doe",
                PerformancesId = request.PerformancesId,
                TotalAmount = 150.00m
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<InvoiceUpdate.Command>(), default))
                .ReturnsAsync(invoiceResponse);

            // Act
            var result = await _controller.InvoiceUpdateAsync(id, request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(invoiceResponse, okResult.Value);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsOkResult_WhenInvoiceIsFound()
        {
            // Arrange
            var id = Guid.NewGuid();
            var invoiceResponse = new InvoiceResponse
            {
                Id = id,
                Customer = "John Doe",
                PerformancesId = new List<Guid> { Guid.NewGuid() },
                TotalAmount = 200.00m
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<InvoiceGetById.Command>(), default))
                .ReturnsAsync(invoiceResponse);

            // Act
            var result = await _controller.GetByIdAsync(id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(invoiceResponse, okResult.Value);
        }

        [Fact]
        public async Task GetInvoiceStatementAsXmlAsync_ReturnsNotFound_WhenInvoiceIsNotFound()
        {
            // Arrange
            var id = Guid.NewGuid();

            _mediatorMock.Setup(m => m.Send(It.IsAny<InvoiceGetByStatement.Command>(), default))
                .ReturnsAsync((InvoiceStatementResponse)null); // Simulando que não foi encontrado

            // Act
            var result = await _controller.GetInvoiceStatementAsXmlAsync(id);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task GetInvoiceStatementAsXmlAsync_ReturnsXmlContent_WhenInvoiceStatementIsFound()
        {
            // Arrange
            var id = Guid.NewGuid();
            var statement = new InvoiceStatementResponse
            {
                Id = id,
                Statement = "Sample statement"
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<InvoiceGetByStatement.Command>(), default))
                .ReturnsAsync(statement);

            // Act
            var result = await _controller.GetInvoiceStatementAsXmlAsync(id);

            // Assert
            var contentResult = Assert.IsType<ContentResult>(result);
            Assert.Equal("application/xml", contentResult.ContentType);
            Assert.Contains("Sample statement", contentResult.Content);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsOkResult_WhenInvoiceIsDeletedSuccessfully()
        {
            // Arrange
            var id = Guid.NewGuid();
            var deleteResponse = new { Success = true };

            _mediatorMock.Setup(m => m.Send(It.IsAny<InvoiceDelete.Command>(), default))
                .ReturnsAsync(deleteResponse);

            // Act
            var result = await _controller.DeleteAsync(id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(deleteResponse, okResult.Value);
        }
    }
}
