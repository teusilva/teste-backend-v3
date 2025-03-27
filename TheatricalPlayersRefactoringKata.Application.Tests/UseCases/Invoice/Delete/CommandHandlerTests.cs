using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using TheatricalPlayersRefactoringKata.Application.UseCases.Invoice.Delete;
using TheatricalPlayersRefactoringKata.Domain.Repositories;

namespace TheatricalPlayersRefactoringKata.Application.Tests.UseCases.Invoice.Delete
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

            _handler = new CommandHandler(
                _loggerMock.Object,
                _repositoryMock.Object,
                _unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldDeleteInvoiceAndReturnId_WhenInvoiceExists()
        {
            var invoiceId = Guid.NewGuid();
            var invoiceEntity = new Domain.Entities.Invoice { Id = invoiceId, Customer = "John Doe" };
            var command = new Command(invoiceId);

            _repositoryMock.Setup(r => r.GetByIdAsync(invoiceId))
                           .ReturnsAsync(invoiceEntity);

            _repositoryMock.Setup(r => r.DeleteAsync(It.IsAny<Domain.Entities.Invoice>()))
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
            var command = new Command(invoiceId);

            _repositoryMock.Setup(r => r.GetByIdAsync(invoiceId))
                           .ReturnsAsync(value: null);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        }
    }
}
