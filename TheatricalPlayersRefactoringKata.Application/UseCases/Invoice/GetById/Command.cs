using MediatR;
using TheatricalPlayersRefactoringKata.Application.DTOs.Response;

namespace TheatricalPlayersRefactoringKata.Application.UseCases.Invoice.GetById
{
    public record Command(Guid id)
    : IRequest<InvoiceResponse>;
}
