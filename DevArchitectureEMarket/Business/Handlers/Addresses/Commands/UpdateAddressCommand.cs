
using Business.Constants;
using Business.BusinessAspects;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Core.Aspects.Autofac.Validation;
using Business.Handlers.Addresses.ValidationRules;


namespace Business.Handlers.Addresses.Commands
{


    public class UpdateAddressCommand : IRequest<IResult>
    {
        public int Id { get; set; }
        public int CityId { get; set; }
        public int UserId { get; set; }
        public int PostalCode { get; set; }
        public string NameSurname { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string AddressTitle { get; set; }
        public string OpenAddress { get; set; }
        public System.DateTime CreateDate { get; set; }
        public bool Active { get; set; }

        public class UpdateAddressCommandHandler : IRequestHandler<UpdateAddressCommand, IResult>
        {
            private readonly IAddressRepository _addressRepository;
            private readonly IMediator _mediator;

            public UpdateAddressCommandHandler(IAddressRepository addressRepository, IMediator mediator)
            {
                _addressRepository = addressRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(UpdateAddressValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(UpdateAddressCommand request, CancellationToken cancellationToken)
            {
                var isThereAddressRecord = await _addressRepository.GetAsync(u => u.Id == request.Id);


                isThereAddressRecord.CityId = request.CityId;
                isThereAddressRecord.UserId = request.UserId;
                isThereAddressRecord.PostalCode = request.PostalCode;
                isThereAddressRecord.NameSurname = request.NameSurname;
                isThereAddressRecord.Phone = request.Phone;
                isThereAddressRecord.Email = request.Email;
                isThereAddressRecord.AddressTitle = request.AddressTitle;
                isThereAddressRecord.OpenAddress = request.OpenAddress;
                isThereAddressRecord.CreateDate = request.CreateDate;
                isThereAddressRecord.Active = request.Active;


                _addressRepository.Update(isThereAddressRecord);
                await _addressRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Updated);
            }
        }
    }
}

