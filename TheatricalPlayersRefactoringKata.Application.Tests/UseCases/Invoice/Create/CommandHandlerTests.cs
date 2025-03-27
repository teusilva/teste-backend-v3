using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using TheatricalPlayersRefactoringKata.Application.DTOs.Request;
using TheatricalPlayersRefactoringKata.Application.UseCases.Invoice.Create;
using TheatricalPlayersRefactoringKata.Domain.Repositories;

namespace TheatricalPlayersRefactoringKata.Application.Tests.UseCases.Invoice.Create
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
        public async Task Handle_ShouldCreateInvoiceAndReturnId()
        {
            // Arrange
            var invoiceId = Guid.NewGuid();
            var request = new InvoiceRequest { Customer = "John Doe", PerformancesId = new List<Guid> { Guid.NewGuid() } };
            var command = new Command(request);

            var invoiceEntity = new Domain.Entities.Invoice { Id = invoiceId, Customer = request.Customer };

            _mapperMock.Setup(m => m.Map<Domain.Entities.Invoice>(command.request))
                       .Returns(invoiceEntity);

            _repositoryMock.Setup(r => r.AddAsync(It.IsAny<Domain.Entities.Invoice>()))
                           .ReturnsAsync(It.IsAny<Domain.Entities.Invoice>());

            _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
                           .Returns(Task.CompletedTask);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.Equal(invoiceId, result);
        }
    }
}
