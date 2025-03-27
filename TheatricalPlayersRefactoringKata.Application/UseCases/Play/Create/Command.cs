using MediatR;
using TheatricalPlayersRefactoringKata.Application.DTOs.Request;

namespace TheatricalPlayersRefactoringKata.Application.UseCases.Play.Create
{
    public record Command(PlayRequest request)
    : IRequest<Guid>;
}
