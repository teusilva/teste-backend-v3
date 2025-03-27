using MediatR;

namespace TheatricalPlayersRefactoringKata.Application.UseCases.Invoice.Delete
{
    public record Command(Guid id)
    : IRequest<Guid>;
}
