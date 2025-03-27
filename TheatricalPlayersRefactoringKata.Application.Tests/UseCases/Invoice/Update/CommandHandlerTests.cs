using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using TheatricalPlayersRefactoringKata.Application.DTOs.Request;
using TheatricalPlayersRefactoringKata.Application.UseCases.Invoice.Update;
using TheatricalPlayersRefactoringKata.Domain.Repositories;

namespace TheatricalPlayersRefactoringKata.Application.Tests.UseCases.Invoice.Update
{
    public class CommandHandlerTests
    {
        private readonly Mock<ILogger<CommandHandler>> _loggerMock;
        private readonly Mock<IInvoiceRepository> _repositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly CommandHandler _handler;

        public CommandHandlerTests()
        {
            _loggerMock = new Mock<ILogger<CommandHandler>>();
            _repositoryMock = new Mock<IInvoiceRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();

            _handler = new CommandHandler(
                _loggerMock.Object,
                _repositoryMock.Object,
                _unitOfWorkMock.Object,
                _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnInvoiceId_WhenInvoiceExists()
        {
            var invoiceId = Guid.NewGuid();
            var invoice = new Domain.Entities.Invoice { Id = invoiceId, Customer = "John Doe" };

            var request = new InvoiceRequest { Customer = "Jane Doe" };
            var command = new Command(invoiceId, request);

            _repositoryMock.Setup(r => r.GetByIdAsync(invoiceId))
                .ReturnsAsync(invoice);

            _mapperMock.Setup(m => m.Map(request, invoice));

            _repositoryMock.Setup(r => r.UpdateAsync(invoice))
                .Returns(Task.CompletedTask);

            _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.Equal(invoiceId, result);
        }

        [Fact]
        public async Task Handle_ShouldThrowKeyNotFoundException_WhenInvoiceDoesNotExist()
        {
            var invoiceId = Guid.NewGuid();
            var request = new InvoiceRequest { Customer = "Jane Doe" };
            var command = new Command(invoiceId, request);

            _repositoryMock.Setup(r => r.GetByIdAsync(invoiceId))
                .ReturnsAsync((Domain.Entities.Invoice)null);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        }
    }
}