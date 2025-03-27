using MediatR;

namespace TheatricalPlayersRefactoringKata.Application.UseCases.Play.Delete
{
    public record Command(Guid id)
    : IRequest<Guid>;
}
