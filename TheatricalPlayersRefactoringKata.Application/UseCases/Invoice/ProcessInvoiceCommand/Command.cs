using MediatR;

namespace TheatricalPlayersRefactoringKata.Application.UseCases.Invoice.ProcessInvoiceCommand
{
    public record Command(string id)
    : IRequest<bool>;
}
