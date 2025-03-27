using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Serialization;
using TheatricalPlayersRefactoringKata.Application.DTOs.Request;
using TheatricalPlayersRefactoringKata.WebApi.Controllers;


namespace TheatricalInvoiceersRefactoringKata.WebApi.Controllers
{
    [ApiController]
    [ApiVersion("1")]
    public class InvoiceController : BaseController<InvoiceController>
    {
        public InvoiceController(IMediator mediator, ILogger<InvoiceController> logger) : base(mediator, logger)
        { }

        [HttpPost]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Post))]
        public async Task<IActionResult> InvoiceCreateAsync([FromBody] InvoiceRequest request)
        {
            var result = await Mediator.Send(new TheatricalPlayersRefactoringKata.Application.UseCases.Invoice.Create.Command(request));
            return Created(string.Empty, result);
        }

        [HttpPost("id/{id}/process")]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Post))]
        public async Task<IActionResult> InvoiceProcessAsync(Guid id)
        {
            var result = await Mediator.Send(new TheatricalPlayersRefactoringKata.Application.UseCases.Invoice.InvoiceProcess.Command(id));
            return Created(string.Empty, result);
        }

        [HttpPut("{id}")]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Put))]
        public async Task<IActionResult> InvoiceUpdateAsync(Guid id, [FromBody] InvoiceRequest request)
        {
            var result = await Mediator.Send(new TheatricalPlayersRefactoringKata.Application.UseCases.Invoice.Update.Command(id, request));
            return Ok(result);
        }

        [HttpGet]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
        public async Task<IActionResult> GetAllAsync([FromQuery] PageOptions request)
        {
            var result = await Mediator.Send(new TheatricalPlayersRefactoringKata.Application.UseCases.Invoice.GetAll.Command(PageOptions: request));
            return Ok(result);
        }

        [HttpGet("{id}")]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Find))]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var result = await Mediator.Send(new TheatricalPlayersRefactoringKata.Application.UseCases.Invoice.GetById.Command(id));
            return Ok(result);
        }

        [HttpGet("{id}/statement")]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Find))]
        public async Task<IActionResult> GetByStatementAsync(Guid id)
        {
            var result = await Mediator.Send(new TheatricalPlayersRefactoringKata.Application.UseCases.Invoice.GetByStatement.Command(id));
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Delete))]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var result = await Mediator.Send(new TheatricalPlayersRefactoringKata.Application.UseCases.Invoice.Delete.Command(id));
            return Ok(result);
        }

        [HttpGet("{id}/statement/xml")]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Find))]
        public async Task<IActionResult> GetInvoiceStatementAsXmlAsync(Guid id)
        {
            // Obtém os dados do extrato
            var result = await Mediator.Send(new TheatricalPlayersRefactoringKata.Application.UseCases.Invoice.GetByStatement.Command(id));

            if (result == null)
            {
                return NotFound();
            }

            // Serializa o resultado em XML
            var xmlString = SerializeToXml(result);

            // Retorna o XML como conteúdo
            return Content(xmlString, "application/xml");
        }

        private string SerializeToXml(object obj)
        {
            var serializer = new XmlSerializer(obj.GetType());
            using (var stringWriter = new StringWriter())
            {
                serializer.Serialize(stringWriter, obj);
                return stringWriter.ToString();
            }
        }
    }
}