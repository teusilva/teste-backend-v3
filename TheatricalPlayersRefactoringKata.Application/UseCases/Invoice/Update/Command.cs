using MediatR;
using TheatricalPlayersRefactoringKata.Application.DTOs.Request;

namespace TheatricalPlayersRefactoringKata.Application.UseCases.Invoice.Update
{
    public record Command(Guid Id, InvoiceRequest request)
    : IRequest<Guid>;
}
