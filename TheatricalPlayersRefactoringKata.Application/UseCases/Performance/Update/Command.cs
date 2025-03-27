using MediatR;
using TheatricalPlayersRefactoringKata.Application.DTOs.Request;

namespace TheatricalPlayersRefactoringKata.Application.UseCases.Performance.Update
{
    public record Command(Guid Id, PerformanceRequest request)
    : IRequest<Guid>;
}
