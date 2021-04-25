
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
using Business.Handlers.OrderDetails.ValidationRules;


namespace Business.Handlers.OrderDetails.Commands
{


    public class UpdateOrderDetailCommand : IRequest<IResult>
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public decimal SalePrice { get; set; }
        public System.DateTime CreateDate { get; set; }
        public bool Active { get; set; }

        public class UpdateOrderDetailCommandHandler : IRequestHandler<UpdateOrderDetailCommand, IResult>
        {
            private readonly IOrderDetailRepository _orderDetailRepository;
            private readonly IMediator _mediator;

            public UpdateOrderDetailCommandHandler(IOrderDetailRepository orderDetailRepository, IMediator mediator)
            {
                _orderDetailRepository = orderDetailRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(UpdateOrderDetailValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(UpdateOrderDetailCommand request, CancellationToken cancellationToken)
            {
                var isThereOrderDetailRecord = await _orderDetailRepository.GetAsync(u => u.Id == request.Id);


                isThereOrderDetailRecord.OrderId = request.OrderId;
                isThereOrderDetailRecord.SalePrice = request.SalePrice;
                isThereOrderDetailRecord.CreateDate = request.CreateDate;
                isThereOrderDetailRecord.Active = request.Active;


                _orderDetailRepository.Update(isThereOrderDetailRecord);
                await _orderDetailRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Updated);
            }
        }
    }
}

