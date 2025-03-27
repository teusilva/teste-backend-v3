using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace TheatricalPlayersRefactoringKata.WebApi.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public abstract class BaseController<T> : ControllerBase
    {
        private IMediator _mediatorInstance;
        private ILogger<T> _loggerInstance;

        protected BaseController(IMediator mediator, ILogger<T> logger)
        {
            _mediatorInstance = mediator;
            _loggerInstance = logger;
        }

        protected IMediator Mediator => _mediatorInstance ??= HttpContext.RequestServices.GetService<IMediator>();
        protected ILogger<T> Logger => _loggerInstance ??= HttpContext.RequestServices.GetService<ILogger<T>>();
    }
}
