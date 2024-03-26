using FluentValidation;
using Kontur.BigLibrary.Service.Contracts;

namespace Kontur.BigLibrary.Service.Validations
{
    public class RubricValidator: AbstractValidator<Rubric>
    {
        public RubricValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Не задано название.");
        }
    }
}