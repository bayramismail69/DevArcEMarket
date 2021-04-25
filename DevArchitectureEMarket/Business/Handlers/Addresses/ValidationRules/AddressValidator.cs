
using Business.Handlers.Addresses.Commands;
using FluentValidation;

namespace Business.Handlers.Addresses.ValidationRules
{

    public class CreateAddressValidator : AbstractValidator<CreateAddressCommand>
    {
        public CreateAddressValidator()
        {
            RuleFor(x => x.CityId).NotEmpty();
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.PostalCode).NotEmpty();
            RuleFor(x => x.NameSurname).NotEmpty();
            RuleFor(x => x.Phone).NotEmpty();
            RuleFor(x => x.Email).NotEmpty();
            RuleFor(x => x.AddressTitle).NotEmpty();
            RuleFor(x => x.OpenAddress).NotEmpty();
            RuleFor(x => x.CreateDate).NotEmpty();
            RuleFor(x => x.Active).NotEmpty();

        }
    }
    public class UpdateAddressValidator : AbstractValidator<UpdateAddressCommand>
    {
        public UpdateAddressValidator()
        {
            RuleFor(x => x.CityId).NotEmpty();
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.PostalCode).NotEmpty();
            RuleFor(x => x.NameSurname).NotEmpty();
            RuleFor(x => x.Phone).NotEmpty();
            RuleFor(x => x.Email).NotEmpty();
            RuleFor(x => x.AddressTitle).NotEmpty();
            RuleFor(x => x.OpenAddress).NotEmpty();
            RuleFor(x => x.CreateDate).NotEmpty();
            RuleFor(x => x.Active).NotEmpty();

        }
    }
}