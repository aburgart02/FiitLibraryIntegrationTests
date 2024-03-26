using System.Linq;
using FluentValidation;
using Kontur.BigLibrary.Service.Contracts;
using Microsoft.AspNetCore.Http;

namespace Kontur.BigLibrary.Service.Validations
{
    public class ImageValidator: AbstractValidator<FormImageFile>
    {
        public ImageValidator()
        {
            RuleFor(x => x.File)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull().WithMessage("Не указан файл.")
                .Must(x => x.Length > 0).WithMessage("Пустой файл.")
                .Must(IsCorrectContentType).WithMessage("Не верный формат файла.");
        }

        private bool IsCorrectContentType(IFormFile formFile) => Constants.ImageContentTypes.Contains(formFile.ContentType);
    }
}