using MediatR;
using TheatricalPlayersRefactoringKata.Application.DTOs.Request;

namespace TheatricalPlayersRefactoringKata.Application.UseCases.Invoice.Create
{
    public record Command(InvoiceRequest request)
    : IRequest<Guid>;
}
