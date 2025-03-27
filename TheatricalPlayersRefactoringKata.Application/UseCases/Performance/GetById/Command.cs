using MediatR;
using TheatricalPlayersRefactoringKata.Application.DTOs.Response;

namespace TheatricalPlayersRefactoringKata.Application.UseCases.Performance.GetById
{
    public record Command(Guid id)
    : IRequest<PerformanceResponse>;
}
