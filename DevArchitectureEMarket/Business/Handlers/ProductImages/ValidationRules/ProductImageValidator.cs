
using Business.Handlers.ProductImages.Commands;
using FluentValidation;

namespace Business.Handlers.ProductImages.ValidationRules
{

    public class CreateProductImageValidator : AbstractValidator<CreateProductImageCommand>
    {
        public CreateProductImageValidator()
        {
            RuleFor(x => x.ProductId).NotEmpty();
            RuleFor(x => x.ImagePath).NotEmpty();
            RuleFor(x => x.DateOfUpload).NotEmpty();

        }
    }
    public class UpdateProductImageValidator : AbstractValidator<UpdateProductImageCommand>
    {
        public UpdateProductImageValidator()
        {
            RuleFor(x => x.ProductId).NotEmpty();
            RuleFor(x => x.ImagePath).NotEmpty();
            RuleFor(x => x.DateOfUpload).NotEmpty();

        }
    }
}