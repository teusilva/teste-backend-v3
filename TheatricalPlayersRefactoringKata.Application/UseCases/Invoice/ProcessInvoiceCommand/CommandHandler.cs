using MediatR;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Xml.Linq;
using TheatricalPlayersRefactoringKata.Domain.Repositories;

namespace TheatricalPlayersRefactoringKata.Application.UseCases.Invoice.ProcessInvoiceCommand
{
    public class CommandHandler : IRequestHandler<Command, bool>
    {
        private readonly ILogger<CommandHandler> _logger;
        private readonly IInvoiceRepository _invoiceRepository;
        public CommandHandler(ILogger<CommandHandler> logger, IInvoiceRepository invoiceRepository)
        {
            _logger = logger;
            _invoiceRepository = invoiceRepository;
        }

        public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
        {
            var invoice = await _invoiceRepository.GetByIdAsync(Guid.Parse(request.id));
            if (invoice is null)
                throw new KeyNotFoundException("Invoice not found");

            var xmlContent = new XDocument(
                new XElement("Invoice",
                    new XElement("Customer", invoice.Customer),
                    new XElement("Performances",
                        invoice.Performances.Select(perf =>
                            new XElement("Performance",
                                new XElement("Play", perf.Play.Name),
                                new XElement("Audience", perf.Audience)
                            )
                        )
                    )
                )
            );

            var path = Path.Combine("Invoices", $"{invoice.Id}.xml");
            Directory.CreateDirectory("Invoices");
            await File.WriteAllTextAsync(path, xmlContent.ToString(), Encoding.UTF8);

            _logger.LogInformation($"Invoice {invoice.Id} processed and saved to {path}");
            return true;
        }
    }
}