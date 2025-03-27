using FluentValidation;

namespace TheatricalPlayersRefactoringKata.Application.UseCases.Invoice.Create
{
    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.request)
                .NotEmpty()
                .NotNull();
        }
    }
}
