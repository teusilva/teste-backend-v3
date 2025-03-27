using MediatR;
using Microsoft.AspNetCore.Mvc;
using TheatricalPlayersRefactoringKata.Application.DTOs.Request;


namespace TheatricalPlayersRefactoringKata.WebApi.Controllers
{
    [ApiController]
    [ApiVersion("1")]
    public class PlayController : BaseController<PlayController>
    {
        public PlayController(IMediator mediator, ILogger<PlayController> logger) : base(mediator, logger)
        { }

        [HttpPost]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Post))]
        public async Task<IActionResult> PlayCreateAsync([FromBody] PlayRequest request)
        {
            var result = await Mediator.Send(new Application.UseCases.Play.Create.Command(request));
            return Created(string.Empty, result);
        }

        [HttpPut("{id}")]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Put))]
        public async Task<IActionResult> PlayUpdateAsync(Guid id, [FromBody] PlayRequest request)
        {
            var result = await Mediator.Send(new Application.UseCases.Play.Update.Command(id, request));
            return Ok(result);
        }

        [HttpGet]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
        public async Task<IActionResult> GetAllAsync([FromQuery] PageOptions request)
        {
            var result = await Mediator.Send(new Application.UseCases.Play.GetAll.Command(PageOptions: request));
            return Ok(result);
        }

        [HttpGet("{id}")]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Find))]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var result = await Mediator.Send(new Application.UseCases.Play.GetById.Command(id));
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Delete))]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var result = await Mediator.Send(new Application.UseCases.Play.Delete.Command(id));
            return Ok(result);
        }
    }
}