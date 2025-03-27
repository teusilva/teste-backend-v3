using MediatR;
using TheatricalPlayersRefactoringKata.Application.DTOs.Request;
using TheatricalPlayersRefactoringKata.Application.DTOs.Response;

namespace TheatricalPlayersRefactoringKata.Application.UseCases.Play.GetAll
{
    public record Command(PageOptions PageOptions)
    : IRequest<List<PlayResponse>>;
}
