using MediatR;

namespace TheatricalPlayersRefactoringKata.Application.UseCases.Performance.Delete
{
    public record Command(Guid id)
    : IRequest<Guid>;
}
