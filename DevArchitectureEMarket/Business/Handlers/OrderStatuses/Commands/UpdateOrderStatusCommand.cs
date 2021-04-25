
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
using Business.Handlers.OrderStatuses.ValidationRules;


namespace Business.Handlers.OrderStatuses.Commands
{


    public class UpdateOrderStatusCommand : IRequest<IResult>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public System.DateTime CreateDate { get; set; }
        public bool Active { get; set; }

        public class UpdateOrderStatusCommandHandler : IRequestHandler<UpdateOrderStatusCommand, IResult>
        {
            private readonly IOrderStatusRepository _orderStatusRepository;
            private readonly IMediator _mediator;

            public UpdateOrderStatusCommandHandler(IOrderStatusRepository orderStatusRepository, IMediator mediator)
            {
                _orderStatusRepository = orderStatusRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(UpdateOrderStatusValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
            {
                var isThereOrderStatusRecord = await _orderStatusRepository.GetAsync(u => u.Id == request.Id);


                isThereOrderStatusRecord.Name = request.Name;
                isThereOrderStatusRecord.CreateDate = request.CreateDate;
                isThereOrderStatusRecord.Active = request.Active;


                _orderStatusRepository.Update(isThereOrderStatusRecord);
                await _orderStatusRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Updated);
            }
        }
    }
}

