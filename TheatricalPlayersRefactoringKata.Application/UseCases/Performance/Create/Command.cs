using MediatR;
using TheatricalPlayersRefactoringKata.Application.DTOs.Request;

namespace TheatricalPlayersRefactoringKata.Application.UseCases.Performance.Create
{
    public record Command(PerformanceRequest request)
    : IRequest<Guid>;
}
