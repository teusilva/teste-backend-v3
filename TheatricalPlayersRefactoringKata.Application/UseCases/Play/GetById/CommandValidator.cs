using FluentValidation;

namespace TheatricalPlayersRefactoringKata.Application.UseCases.Play.GetById
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
