using MediatR;
using TheatricalPlayersRefactoringKata.Application.DTOs.Request;
using TheatricalPlayersRefactoringKata.Application.DTOs.Response;

namespace TheatricalPlayersRefactoringKata.Application.UseCases.Performance.GetAll
{
    public record Command(PageOptions PageOptions)
    : IRequest<List<PerformanceResponse>>;
}
