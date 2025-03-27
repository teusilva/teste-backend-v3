using FluentValidation;

namespace TheatricalPlayersRefactoringKata.Application.UseCases.Invoice.InvoiceProcess
{
    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.id)
                .NotEmpty()
                .NotNull();
        }
    }
}
