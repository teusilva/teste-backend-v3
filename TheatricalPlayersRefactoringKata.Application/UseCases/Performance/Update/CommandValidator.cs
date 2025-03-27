using FluentValidation;

namespace TheatricalPlayersRefactoringKata.Application.UseCases.Performance.Update
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
