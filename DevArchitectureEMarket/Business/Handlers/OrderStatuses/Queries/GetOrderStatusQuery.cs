
using Business.BusinessAspects;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;


namespace Business.Handlers.OrderStatuses.Queries
{
    public class GetOrderStatusQuery : IRequest<IDataResult<OrderStatus>>
    {
        public int Id { get; set; }

        public class GetOrderStatusQueryHandler : IRequestHandler<GetOrderStatusQuery, IDataResult<OrderStatus>>
        {
            private readonly IOrderStatusRepository _orderStatusRepository;
            private readonly IMediator _mediator;

            public GetOrderStatusQueryHandler(IOrderStatusRepository orderStatusRepository, IMediator mediator)
            {
                _orderStatusRepository = orderStatusRepository;
                _mediator = mediator;
            }
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<OrderStatus>> Handle(GetOrderStatusQuery request, CancellationToken cancellationToken)
            {
                var orderStatus = await _orderStatusRepository.GetAsync(p => p.Id == request.Id);
                return new SuccessDataResult<OrderStatus>(orderStatus);
            }
        }
    }
}
