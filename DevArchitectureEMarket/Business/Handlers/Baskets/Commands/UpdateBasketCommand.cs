
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
using Business.Handlers.Baskets.ValidationRules;


namespace Business.Handlers.Baskets.Commands
{


    public class UpdateBasketCommand : IRequest<IResult>
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int PaymentMethodId { get; set; }
        public int Count { get; set; }
        public System.DateTime CreateDate { get; set; }
        public bool Active { get; set; }

        public class UpdateBasketCommandHandler : IRequestHandler<UpdateBasketCommand, IResult>
        {
            private readonly IBasketRepository _basketRepository;
            private readonly IMediator _mediator;

            public UpdateBasketCommandHandler(IBasketRepository basketRepository, IMediator mediator)
            {
                _basketRepository = basketRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(UpdateBasketValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(UpdateBasketCommand request, CancellationToken cancellationToken)
            {
                var isThereBasketRecord = await _basketRepository.GetAsync(u => u.Id == request.Id);


                isThereBasketRecord.UserId = request.UserId;
                isThereBasketRecord.ProductId = request.ProductId;
                isThereBasketRecord.PaymentMethodId = request.PaymentMethodId;
                isThereBasketRecord.Count = request.Count;
                isThereBasketRecord.CreateDate = request.CreateDate;
                isThereBasketRecord.Active = request.Active;


                _basketRepository.Update(isThereBasketRecord);
                await _basketRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Updated);
            }
        }
    }
}

