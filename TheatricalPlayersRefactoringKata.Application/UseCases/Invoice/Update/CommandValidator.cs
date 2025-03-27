using FluentValidation;

namespace TheatricalPlayersRefactoringKata.Application.UseCases.Invoice.Update
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
