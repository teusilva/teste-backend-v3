using FluentValidation;

namespace TheatricalPlayersRefactoringKata.Application.UseCases.Play.Create
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
