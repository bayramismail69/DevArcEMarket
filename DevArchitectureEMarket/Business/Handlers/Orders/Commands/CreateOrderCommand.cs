
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
using Business.Handlers.Orders.ValidationRules;

namespace Business.Handlers.Orders.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateOrderCommand : IRequest<IResult>
    {

        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int AddressId { get; set; }
        public int OrderStatusId { get; set; }
        public int PaymentMethodId { get; set; }
        public int Count { get; set; }
        public System.DateTime CreateDate { get; set; }
        public bool Active { get; set; }


        public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, IResult>
        {
            private readonly IOrderRepository _orderRepository;
            private readonly IMediator _mediator;
            public CreateOrderCommandHandler(IOrderRepository orderRepository, IMediator mediator)
            {
                _orderRepository = orderRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(CreateOrderValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
            {
                var isThereOrderRecord = _orderRepository.Query().Any(u => u.UserId == request.UserId);

                if (isThereOrderRecord == true)
                    return new ErrorResult(Messages.NameAlreadyExist);

                var addedOrder = new Order
                {
                    UserId = request.UserId,
                    ProductId = request.ProductId,
                    AddressId = request.AddressId,
                    OrderStatusId = request.OrderStatusId,
                    PaymentMethodId = request.PaymentMethodId,
                    Count = request.Count,
                    CreateDate = request.CreateDate,
                    Active = request.Active,

                };

                _orderRepository.Add(addedOrder);
                await _orderRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Added);
            }
        }
    }
}