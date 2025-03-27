using FluentValidation;

namespace TheatricalPlayersRefactoringKata.Application.UseCases.Performance.GetById
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
