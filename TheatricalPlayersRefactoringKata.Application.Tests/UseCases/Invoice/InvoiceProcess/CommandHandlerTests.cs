using Microsoft.Extensions.Logging;
using Moq;
using TheatricalPlayersRefactoringKata.Application.ExternalServices.RabbitMQ.Publisher;
using TheatricalPlayersRefactoringKata.Application.UseCases.Invoice.InvoiceProcess;

namespace TheatricalPlayersRefactoringKata.Application.Tests.UseCases.Invoice.InvoiceProcess
{
    public class CommandHandlerTests
    {
        private readonly Mock<ILogger<CommandHandler>> _loggerMock;
        private readonly Mock<IInvoicePublisher> _repositoryMock;
        private readonly CommandHandler _handler;

        public CommandHandlerTests()
        {
            _loggerMock = new Mock<ILogger<CommandHandler>>();
            _repositoryMock = new Mock<IInvoicePublisher>();

            _handler = new CommandHandler(
                _loggerMock.Object,
                _repositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnInvoiceResponse_WhenInvoiceExists()
        {
            // Arrange
            var invoiceId = Guid.NewGuid();
            var command = new Command(invoiceId);

            _repositoryMock.Setup(r => r.Publish(invoiceId));

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);

        }
    }
}
