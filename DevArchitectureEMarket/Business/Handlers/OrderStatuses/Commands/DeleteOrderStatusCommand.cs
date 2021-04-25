
using Business.Constants;
using Core.Aspects.Autofac.Caching;
using Business.BusinessAspects;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using MediatR;
using System.Threading;
using System.Threading.Tasks;


namespace Business.Handlers.OrderStatuses.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class DeleteOrderStatusCommand : IRequest<IResult>
    {
        public int Id { get; set; }

        public class DeleteOrderStatusCommandHandler : IRequestHandler<DeleteOrderStatusCommand, IResult>
        {
            private readonly IOrderStatusRepository _orderStatusRepository;
            private readonly IMediator _mediator;

            public DeleteOrderStatusCommandHandler(IOrderStatusRepository orderStatusRepository, IMediator mediator)
            {
                _orderStatusRepository = orderStatusRepository;
                _mediator = mediator;
            }

            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(DeleteOrderStatusCommand request, CancellationToken cancellationToken)
            {
                var orderStatusToDelete = _orderStatusRepository.Get(p => p.Id == request.Id);

                _orderStatusRepository.Delete(orderStatusToDelete);
                await _orderStatusRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Deleted);
            }
        }
    }
}

