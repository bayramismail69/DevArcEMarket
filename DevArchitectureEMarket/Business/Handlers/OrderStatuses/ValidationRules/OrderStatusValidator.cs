
using Business.Handlers.OrderStatuses.Commands;
using FluentValidation;

namespace Business.Handlers.OrderStatuses.ValidationRules
{

    public class CreateOrderStatusValidator : AbstractValidator<CreateOrderStatusCommand>
    {
        public CreateOrderStatusValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.CreateDate).NotEmpty();
            RuleFor(x => x.Active).NotEmpty();

        }
    }
    public class UpdateOrderStatusValidator : AbstractValidator<UpdateOrderStatusCommand>
    {
        public UpdateOrderStatusValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.CreateDate).NotEmpty();
            RuleFor(x => x.Active).NotEmpty();

        }
    }
}