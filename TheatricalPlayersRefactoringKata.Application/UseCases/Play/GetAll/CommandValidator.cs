using FluentValidation;

namespace TheatricalPlayersRefactoringKata.Application.UseCases.Play.GetAll
{
    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.PageOptions.Page)
                .NotEmpty()
                .NotNull();

            RuleFor(x => x.PageOptions.PageSize)
                .NotEmpty()
                .NotNull();
        }
    }
}
