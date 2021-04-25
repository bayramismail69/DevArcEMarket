
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


namespace Business.Handlers.ProductImages.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class DeleteProductImageCommand : IRequest<IResult>
    {
        public int Id { get; set; }

        public class DeleteProductImageCommandHandler : IRequestHandler<DeleteProductImageCommand, IResult>
        {
            private readonly IProductImageRepository _productImageRepository;
            private readonly IMediator _mediator;

            public DeleteProductImageCommandHandler(IProductImageRepository productImageRepository, IMediator mediator)
            {
                _productImageRepository = productImageRepository;
                _mediator = mediator;
            }

            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(DeleteProductImageCommand request, CancellationToken cancellationToken)
            {
                var productImageToDelete = _productImageRepository.Get(p => p.Id == request.Id);

                _productImageRepository.Delete(productImageToDelete);
                await _productImageRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Deleted);
            }
        }
    }
}

