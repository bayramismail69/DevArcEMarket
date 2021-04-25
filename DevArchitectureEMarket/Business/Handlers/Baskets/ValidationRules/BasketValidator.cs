
using Business.Handlers.Baskets.Commands;
using FluentValidation;

namespace Business.Handlers.Baskets.ValidationRules
{

    public class CreateBasketValidator : AbstractValidator<CreateBasketCommand>
    {
        public CreateBasketValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.ProductId).NotEmpty();
            RuleFor(x => x.PaymentMethodId).NotEmpty();
            RuleFor(x => x.Count).NotEmpty();
            RuleFor(x => x.CreateDate).NotEmpty();
            RuleFor(x => x.Active).NotEmpty();

        }
    }
    public class UpdateBasketValidator : AbstractValidator<UpdateBasketCommand>
    {
        public UpdateBasketValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.ProductId).NotEmpty();
            RuleFor(x => x.PaymentMethodId).NotEmpty();
            RuleFor(x => x.Count).NotEmpty();
            RuleFor(x => x.CreateDate).NotEmpty();
            RuleFor(x => x.Active).NotEmpty();

        }
    }
}