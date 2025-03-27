using Microsoft.Extensions.Logging;
using Moq;
using TheatricalPlayersRefactoringKata.Application.UseCases.Invoice.GetByStatement;
using TheatricalPlayersRefactoringKata.Domain.Enums;
using TheatricalPlayersRefactoringKata.Domain.Repositories;

namespace TheatricalPlayersRefactoringKata.Application.Tests.UseCases.Invoice.GetByStatement
{
    public class CommandHandlerTests
    {
        private readonly Mock<ILogger<CommandHandler>> _loggerMock;
        private readonly Mock<IInvoiceRepository> _repositoryMock;
        private readonly CommandHandler _handler;

        public CommandHandlerTests()
        {
            _loggerMock = new Mock<ILogger<CommandHandler>>();
            _repositoryMock = new Mock<IInvoiceRepository>();

            _handler = new CommandHandler(
                _loggerMock.Object,
                _repositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnFormattedStatement_WhenInvoiceExists()
        {
            var invoiceId = Guid.NewGuid();
            var play = new Domain.Entities.Play { Id = Guid.NewGuid(), Name = "Hamlet", Type = TypesPlays.Tragedy, Lines = 3000 };
            var performance = new Domain.Entities.Performance { Id = Guid.NewGuid(), Play = play, Audience = 35 };

            var invoice = new Domain.Entities.Invoice
            {
                Id = invoiceId,
                Customer = "John Doe",
                Performances = new List<Domain.Entities.Performance> { performance }
            };

            var command = new Command(invoiceId);

            _repositoryMock.Setup(r => r.GetByIdThenIncludeAsync(invoiceId))
                .ReturnsAsync(invoice);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.Contains("Statement for John Doe", result);
            Assert.Contains("Hamlet", result);
            Assert.Contains("Amount owed is", result);
            Assert.Contains("You earned", result);
        }

        [Fact]
        public async Task Handle_ShouldThrowKeyNotFoundException_WhenInvoiceDoesNotExist()
        {
            var invoiceId = Guid.NewGuid();
            var command = new Command(invoiceId);

            _repositoryMock.Setup(r => r.GetByIdThenIncludeAsync(invoiceId))
                .ReturnsAsync((Domain.Entities.Invoice)null);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        }
    }
}
