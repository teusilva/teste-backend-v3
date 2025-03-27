using MediatR;
using TheatricalPlayersRefactoringKata.Application.DTOs.Response;

namespace TheatricalPlayersRefactoringKata.Application.UseCases.Play.GetById
{
    public record Command(Guid id)
    : IRequest<PlayResponse>;
}
