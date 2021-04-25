
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
using Business.Handlers.OrderStatuses.ValidationRules;

namespace Business.Handlers.OrderStatuses.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateOrderStatusCommand : IRequest<IResult>
    {

        public string Name { get; set; }
        public System.DateTime CreateDate { get; set; }
        public bool Active { get; set; }


        public class CreateOrderStatusCommandHandler : IRequestHandler<CreateOrderStatusCommand, IResult>
        {
            private readonly IOrderStatusRepository _orderStatusRepository;
            private readonly IMediator _mediator;
            public CreateOrderStatusCommandHandler(IOrderStatusRepository orderStatusRepository, IMediator mediator)
            {
                _orderStatusRepository = orderStatusRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(CreateOrderStatusValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(CreateOrderStatusCommand request, CancellationToken cancellationToken)
            {
                var isThereOrderStatusRecord = _orderStatusRepository.Query().Any(u => u.Name == request.Name);

                if (isThereOrderStatusRecord == true)
                    return new ErrorResult(Messages.NameAlreadyExist);

                var addedOrderStatus = new OrderStatus
                {
                    Name = request.Name,
                    CreateDate = request.CreateDate,
                    Active = request.Active,

                };

                _orderStatusRepository.Add(addedOrderStatus);
                await _orderStatusRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Added);
            }
        }
    }
}