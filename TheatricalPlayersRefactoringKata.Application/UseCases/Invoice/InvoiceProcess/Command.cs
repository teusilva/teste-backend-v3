using MediatR;
using TheatricalPlayersRefactoringKata.Application.DTOs.Request;

namespace TheatricalPlayersRefactoringKata.Application.UseCases.Invoice.InvoiceProcess
{
    public record Command(Guid id)
    : IRequest<Guid>;
}
