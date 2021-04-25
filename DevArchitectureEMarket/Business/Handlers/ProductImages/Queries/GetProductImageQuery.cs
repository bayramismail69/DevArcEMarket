
using Business.BusinessAspects;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;


namespace Business.Handlers.ProductImages.Queries
{
    public class GetProductImageQuery : IRequest<IDataResult<ProductImage>>
    {
        public int Id { get; set; }

        public class GetProductImageQueryHandler : IRequestHandler<GetProductImageQuery, IDataResult<ProductImage>>
        {
            private readonly IProductImageRepository _productImageRepository;
            private readonly IMediator _mediator;

            public GetProductImageQueryHandler(IProductImageRepository productImageRepository, IMediator mediator)
            {
                _productImageRepository = productImageRepository;
                _mediator = mediator;
            }
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<ProductImage>> Handle(GetProductImageQuery request, CancellationToken cancellationToken)
            {
                var productImage = await _productImageRepository.GetAsync(p => p.Id == request.Id);
                return new SuccessDataResult<ProductImage>(productImage);
            }
        }
    }
}
