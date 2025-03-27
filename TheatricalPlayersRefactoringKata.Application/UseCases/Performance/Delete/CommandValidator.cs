using FluentValidation;

namespace TheatricalPlayersRefactoringKata.Application.UseCases.Performance.Delete
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
