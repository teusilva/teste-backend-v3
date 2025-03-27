using Microsoft.Extensions.Logging;
using Moq;
using System.Text;
using System.Xml.Linq;
using TheatricalPlayersRefactoringKata.Application.UseCases.Invoice.ProcessInvoiceCommand;
using TheatricalPlayersRefactoringKata.Domain.Enums;
using TheatricalPlayersRefactoringKata.Domain.Repositories;

namespace TheatricalPlayersRefactoringKata.Application.Tests.UseCases.Invoice.ProcessInvoiceCommand
{
    public class CommandHandlerTests
    {
        private readonly Mock<IInvoiceRepository> _invoiceRepositoryMock;
        private readonly Mock<ILogger<CommandHandler>> _loggerMock;
        private readonly CommandHandler _handler;

        public CommandHandlerTests()
        {
            _invoiceRepositoryMock = new Mock<IInvoiceRepository>();
            _loggerMock = new Mock<ILogger<CommandHandler>>();
            _handler = new CommandHandler(_loggerMock.Object, _invoiceRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldGenerateXml_WhenInvoiceExists()
        {
            // Arrange
            var invoiceId = Guid.NewGuid();
            var command = new Command(invoiceId.ToString());

            var invoice = new Domain.Entities.Invoice()
            {
                Id = invoiceId,
                Customer = "John Doe",
                Performances = new List<Domain.Entities.Performance>
                {
                    new Domain.Entities.Performance
                    {
                        Audience =1 ,
                        Id = Guid.NewGuid(),
                       Play = new Domain.Entities.Play
                       {
                           Id = Guid.NewGuid(),
                           Lines = 2,
                           Name = "TERSTE",
                           Type = TypesPlays.Tragedy
                       }
                    }
                }
            };

            _invoiceRepositoryMock
                .Setup(repo => repo.GetByIdAsync(invoiceId))
                .ReturnsAsync(invoice);

            var path = Path.Combine("Invoices", $"{invoiceId}.xml");
            if (File.Exists(path)) File.Delete(path);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result);
            Assert.True(File.Exists(path));

            var xmlContent = await File.ReadAllTextAsync(path, Encoding.UTF8);
            var xmlDocument = XDocument.Parse(xmlContent);
            Assert.Equal("John Doe", xmlDocument.Root.Element("Customer")?.Value);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenInvoiceNotFound()
        {
            var invoiceId = Guid.NewGuid();
            var command = new Command(invoiceId.ToString());

            _invoiceRepositoryMock
                .Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Domain.Entities.Invoice)null);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldReturnFalse_WhenIOExceptionOccurs()
        {
            var invoiceId = Guid.NewGuid();
            var command = new Command (invoiceId.ToString());

            var invoice = new Domain.Entities.Invoice()
            {
                Id = invoiceId,
                Customer = "John Doe",
                Performances = new List<Domain.Entities.Performance>
                {
                    new Domain.Entities.Performance
                    {
                        Audience =1 ,
                        Id = Guid.NewGuid(),
                       Play = new Domain.Entities.Play
                       {
                           Id = Guid.NewGuid(),
                           Lines = 2,
                           Name = "TERSTE",
                           Type = TypesPlays.Tragedy
                       }
                    }
                }
            };


            _invoiceRepositoryMock
                .Setup(repo => repo.GetByIdAsync(invoiceId))
                .ReturnsAsync(invoice);

            var filePath = Path.Combine("Invoices", $"{invoiceId}.xml");

            // Criar um arquivo bloqueado para simular erro de escrita
            using (var stream = File.Create(filePath)) { }
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result);
        }
    }
}
