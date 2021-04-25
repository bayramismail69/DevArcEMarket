﻿
using Business.BusinessAspects;
using Business.Constants;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Business.Handlers.Addresses.ValidationRules;

namespace Business.Handlers.Addresses.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateAddressCommand : IRequest<IResult>
    {

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


        public class CreateAddressCommandHandler : IRequestHandler<CreateAddressCommand, IResult>
        {
            private readonly IAddressRepository _addressRepository;
            private readonly IMediator _mediator;
            public CreateAddressCommandHandler(IAddressRepository addressRepository, IMediator mediator)
            {
                _addressRepository = addressRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(CreateAddressValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(CreateAddressCommand request, CancellationToken cancellationToken)
            {
                var isThereAddressRecord = _addressRepository.Query().Any(u => u.CityId == request.CityId);

                if (isThereAddressRecord == true)
                    return new ErrorResult(Messages.NameAlreadyExist);

                var addedAddress = new Address
                {
                    CityId = request.CityId,
                    UserId = request.UserId,
                    PostalCode = request.PostalCode,
                    NameSurname = request.NameSurname,
                    Phone = request.Phone,
                    Email = request.Email,
                    AddressTitle = request.AddressTitle,
                    OpenAddress = request.OpenAddress,
                    CreateDate = request.CreateDate,
                    Active = request.Active,

                };

                _addressRepository.Add(addedAddress);
                await _addressRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Added);
            }
        }
    }
}