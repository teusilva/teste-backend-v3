using FluentValidation;

namespace TheatricalPlayersRefactoringKata.Application.UseCases.Invoice.GetById
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
