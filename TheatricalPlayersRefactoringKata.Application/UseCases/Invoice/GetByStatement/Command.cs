using MediatR;
using TheatricalPlayersRefactoringKata.Application.DTOs.Response;

namespace TheatricalPlayersRefactoringKata.Application.UseCases.Invoice.GetByStatement
{
    public record Command(Guid id)
    : IRequest<string>;
}
