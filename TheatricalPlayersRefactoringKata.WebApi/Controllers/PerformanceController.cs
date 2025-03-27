using MediatR;
using Microsoft.AspNetCore.Mvc;
using TheatricalPlayersRefactoringKata.Application.DTOs.Request;
using TheatricalPlayersRefactoringKata.WebApi.Controllers;


namespace TheatricalPerformanceersRefactoringKata.WebApi.Controllers
{
    [ApiController]
    [ApiVersion("1")]
    public class PerformanceController : BaseController<PerformanceController>
    {
        public PerformanceController(IMediator mediator, ILogger<PerformanceController> logger) : base(mediator, logger)
        { }

        [HttpPost]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Post))]
        public async Task<IActionResult> PerformanceCreateAsync([FromBody] PerformanceRequest request)
        {
            var result = await Mediator.Send(new TheatricalPlayersRefactoringKata.Application.UseCases.Performance.Create.Command(request));
            return Created(string.Empty, result);
        }

        [HttpPut("{id}")]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Put))]
        public async Task<IActionResult> PerformanceUpdateAsync(Guid id, [FromBody] PerformanceRequest request)
        {
            var result = await Mediator.Send(new TheatricalPlayersRefactoringKata.Application.UseCases.Performance.Update.Command(id, request));
            return Ok(result);
        }

        [HttpGet]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
        public async Task<IActionResult> GetAllAsync([FromQuery] PageOptions request)
        {
            var result = await Mediator.Send(new TheatricalPlayersRefactoringKata.Application.UseCases.Performance.GetAll.Command(PageOptions: request));
            return Ok(result);
        }

        [HttpGet("{id}")]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Find))]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var result = await Mediator.Send(new TheatricalPlayersRefactoringKata.Application.UseCases.Performance.GetById.Command(id));
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Delete))]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var result = await Mediator.Send(new TheatricalPlayersRefactoringKata.Application.UseCases.Performance.Delete.Command(id));
            return Ok(result);
        }
    }
}