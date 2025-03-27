using MediatR;
using TheatricalPlayersRefactoringKata.Application.DTOs.Request;

namespace TheatricalPlayersRefactoringKata.Application.UseCases.Play.Update
{
    public record Command(Guid Id, PlayRequest request)
    : IRequest<Guid>;
}
