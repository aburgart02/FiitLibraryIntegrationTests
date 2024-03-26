using FluentValidation;
using Kontur.BigLibrary.Service.Contracts;

namespace Kontur.BigLibrary.Service.Validations
{
    public class BookValidator : AbstractValidator<Book>
    {
        public BookValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Не задано название.");
            RuleFor(x => x.Author).NotEmpty().WithMessage("Не задан автор.");
        }
    }
}