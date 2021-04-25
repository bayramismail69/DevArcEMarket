
using Business.BusinessAspects;
using Core.Aspects.Autofac.Performance;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Aspects.Autofac.Caching;

namespace Business.Handlers.OrderStatuses.Queries
{

    public class GetOrderStatusesQuery : IRequest<IDataResult<IEnumerable<OrderStatus>>>
    {
        public class GetOrderStatusesQueryHandler : IRequestHandler<GetOrderStatusesQuery, IDataResult<IEnumerable<OrderStatus>>>
        {
            private readonly IOrderStatusRepository _orderStatusRepository;
            private readonly IMediator _mediator;

            public GetOrderStatusesQueryHandler(IOrderStatusRepository orderStatusRepository, IMediator mediator)
            {
                _orderStatusRepository = orderStatusRepository;
                _mediator = mediator;
            }

            [PerformanceAspect(5)]
            [CacheAspect(10)]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<IEnumerable<OrderStatus>>> Handle(GetOrderStatusesQuery request, CancellationToken cancellationToken)
            {
                return new SuccessDataResult<IEnumerable<OrderStatus>>(await _orderStatusRepository.GetListAsync());
            }
        }
    }
}