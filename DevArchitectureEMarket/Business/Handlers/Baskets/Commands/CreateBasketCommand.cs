
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
using Business.Handlers.Baskets.ValidationRules;

namespace Business.Handlers.Baskets.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateBasketCommand : IRequest<IResult>
    {

        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int PaymentMethodId { get; set; }
        public int Count { get; set; }
        public System.DateTime CreateDate { get; set; }
        public bool Active { get; set; }


        public class CreateBasketCommandHandler : IRequestHandler<CreateBasketCommand, IResult>
        {
            private readonly IBasketRepository _basketRepository;
            private readonly IMediator _mediator;
            public CreateBasketCommandHandler(IBasketRepository basketRepository, IMediator mediator)
            {
                _basketRepository = basketRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(CreateBasketValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(CreateBasketCommand request, CancellationToken cancellationToken)
            {
                var isThereBasketRecord = _basketRepository.Query().Any(u => u.UserId == request.UserId);

                if (isThereBasketRecord == true)
                    return new ErrorResult(Messages.NameAlreadyExist);

                var addedBasket = new Basket
                {
                    UserId = request.UserId,
                    ProductId = request.ProductId,
                    PaymentMethodId = request.PaymentMethodId,
                    Count = request.Count,
                    CreateDate = request.CreateDate,
                    Active = request.Active,

                };

                _basketRepository.Add(addedBasket);
                await _basketRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Added);
            }
        }
    }
}