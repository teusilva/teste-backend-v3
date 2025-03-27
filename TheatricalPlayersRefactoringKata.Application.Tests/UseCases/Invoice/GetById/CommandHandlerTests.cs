using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using TheatricalPlayersRefactoringKata.Application.DTOs.Response;
using TheatricalPlayersRefactoringKata.Application.UseCases.Invoice.GetById;
using TheatricalPlayersRefactoringKata.Domain.Repositories;

namespace TheatricalPlayersRefactoringKata.Application.Tests.UseCases.Invoice.GetById
{
    public class CommandHandlerTests
    {
        private readonly Mock<ILogger<CommandHandler>> _loggerMock;
        private readonly Mock<IInvoiceRepository> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly CommandHandler _handler;

        public CommandHandlerTests()
        {
            _loggerMock = new Mock<ILogger<CommandHandler>>();
            _repositoryMock = new Mock<IInvoiceRepository>();
            _mapperMock = new Mock<IMapper>();

            _handler = new CommandHandler(
                _loggerMock.Object,
                _repositoryMock.Object,
                _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnInvoiceResponse_WhenInvoiceExists()
        {
            // Arrange
            var invoiceId = Guid.NewGuid();
            var invoice = new Domain.Entities.Invoice { Id = invoiceId, Customer = "John Doe" };
            var invoiceResponse = new InvoiceResponse { Id = invoiceId, Customer = "John Doe" };

            var command = new Command(invoiceId);

            _repositoryMock.Setup(r => r.GetByIdAsync(invoiceId))
                .ReturnsAsync(invoice);

            _mapperMock.Setup(m => m.Map<InvoiceResponse>(invoice))
                .Returns(invoiceResponse);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(invoiceResponse.Id, result.Id);
            Assert.Equal(invoiceResponse.Customer, result.Customer);

        }

        [Fact]
        public async Task Handle_ShouldThrowKeyNotFoundException_WhenInvoiceDoesNotExist()
        {
            // Arrange
            var invoiceId = Guid.NewGuid();
            var command = new Command(invoiceId);

            _repositoryMock.Setup(r => r.GetByIdAsync(invoiceId))
                .ReturnsAsync((Domain.Entities.Invoice)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        }
    }
}
