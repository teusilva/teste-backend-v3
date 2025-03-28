using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using TheatricalInvoiceersRefactoringKata.WebApi.Controllers;
using TheatricalPlayersRefactoringKata.Application.DTOs.Request;
using TheatricalPlayersRefactoringKata.Application.DTOs.Response;

namespace TheatricalPlayersRefactoringKata.WebApi.Tests
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
            var id = Guid.NewGuid();
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
                
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<Application.UseCases.Invoice.Create.Command>(), default))
                .ReturnsAsync(id);

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
                
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<Application.UseCases.Invoice.Update.Command>(), default))
                .ReturnsAsync(id);

            var result = await _controller.InvoiceUpdateAsync(id, request);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(id, okResult.Value);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsOkResult_WhenInvoiceIsFound()
        {
            var id = Guid.NewGuid();
            var invoiceResponse = new InvoiceResponse
            {
                Id = id,
                Customer = "John Doe",
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<Application.UseCases.Invoice.GetById.Command>(), default))
                .ReturnsAsync(invoiceResponse);

            var result = await _controller.GetByIdAsync(id);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(invoiceResponse, okResult.Value);
        }


        [Fact]
        public async Task DeleteAsync_ReturnsOkResult_WhenInvoiceIsDeletedSuccessfully()
        {
            var id = Guid.NewGuid();
            var deleteResponse = new { Success = true };

            _mediatorMock.Setup(m => m.Send(It.IsAny<Application.UseCases.Invoice.Delete.Command>(), default))
                .ReturnsAsync(id);

            var result = await _controller.DeleteAsync(id);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(id, okResult.Value);
        }
    }
}