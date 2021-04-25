
using Business.Handlers.OrderDetails.Commands;
using FluentValidation;

namespace Business.Handlers.OrderDetails.ValidationRules
{

    public class CreateOrderDetailValidator : AbstractValidator<CreateOrderDetailCommand>
    {
        public CreateOrderDetailValidator()
        {
            RuleFor(x => x.OrderId).NotEmpty();
            RuleFor(x => x.SalePrice).NotEmpty();
            RuleFor(x => x.CreateDate).NotEmpty();
            RuleFor(x => x.Active).NotEmpty();

        }
    }
    public class UpdateOrderDetailValidator : AbstractValidator<UpdateOrderDetailCommand>
    {
        public UpdateOrderDetailValidator()
        {
            RuleFor(x => x.OrderId).NotEmpty();
            RuleFor(x => x.SalePrice).NotEmpty();
            RuleFor(x => x.CreateDate).NotEmpty();
            RuleFor(x => x.Active).NotEmpty();

        }
    }
}