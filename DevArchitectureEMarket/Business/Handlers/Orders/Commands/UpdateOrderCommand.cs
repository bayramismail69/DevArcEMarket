
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
using Business.Handlers.Orders.ValidationRules;


namespace Business.Handlers.Orders.Commands
{


    public class UpdateOrderCommand : IRequest<IResult>
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int AddressId { get; set; }
        public int OrderStatusId { get; set; }
        public int PaymentMethodId { get; set; }
        public int Count { get; set; }
        public System.DateTime CreateDate { get; set; }
        public bool Active { get; set; }

        public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, IResult>
        {
            private readonly IOrderRepository _orderRepository;
            private readonly IMediator _mediator;

            public UpdateOrderCommandHandler(IOrderRepository orderRepository, IMediator mediator)
            {
                _orderRepository = orderRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(UpdateOrderValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
            {
                var isThereOrderRecord = await _orderRepository.GetAsync(u => u.Id == request.Id);


                isThereOrderRecord.UserId = request.UserId;
                isThereOrderRecord.ProductId = request.ProductId;
                isThereOrderRecord.AddressId = request.AddressId;
                isThereOrderRecord.OrderStatusId = request.OrderStatusId;
                isThereOrderRecord.PaymentMethodId = request.PaymentMethodId;
                isThereOrderRecord.Count = request.Count;
                isThereOrderRecord.CreateDate = request.CreateDate;
                isThereOrderRecord.Active = request.Active;


                _orderRepository.Update(isThereOrderRecord);
                await _orderRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Updated);
            }
        }
    }
}

