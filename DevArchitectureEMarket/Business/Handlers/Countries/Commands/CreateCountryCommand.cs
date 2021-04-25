
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
using Business.Handlers.Countries.ValidationRules;

namespace Business.Handlers.Countries.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateCountryCommand : IRequest<IResult>
    {

        public string Name { get; set; }
        public System.DateTime CreateDate { get; set; }
        public bool Active { get; set; }


        public class CreateCountryCommandHandler : IRequestHandler<CreateCountryCommand, IResult>
        {
            private readonly ICountryRepository _countryRepository;
            private readonly IMediator _mediator;
            public CreateCountryCommandHandler(ICountryRepository countryRepository, IMediator mediator)
            {
                _countryRepository = countryRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(CreateCountryValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(CreateCountryCommand request, CancellationToken cancellationToken)
            {
                var isThereCountryRecord = _countryRepository.Query().Any(u => u.Name == request.Name);

                if (isThereCountryRecord == true)
                    return new ErrorResult(Messages.NameAlreadyExist);

                var addedCountry = new Country
                {
                    Name = request.Name,
                    CreateDate = request.CreateDate,
                    Active = request.Active,

                };

                _countryRepository.Add(addedCountry);
                await _countryRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Added);
            }
        }
    }
}