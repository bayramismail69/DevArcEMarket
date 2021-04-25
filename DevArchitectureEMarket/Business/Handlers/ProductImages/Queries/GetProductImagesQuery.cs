
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

namespace Business.Handlers.ProductImages.Queries
{

    public class GetProductImagesQuery : IRequest<IDataResult<IEnumerable<ProductImage>>>
    {
        public class GetProductImagesQueryHandler : IRequestHandler<GetProductImagesQuery, IDataResult<IEnumerable<ProductImage>>>
        {
            private readonly IProductImageRepository _productImageRepository;
            private readonly IMediator _mediator;

            public GetProductImagesQueryHandler(IProductImageRepository productImageRepository, IMediator mediator)
            {
                _productImageRepository = productImageRepository;
                _mediator = mediator;
            }

            [PerformanceAspect(5)]
            [CacheAspect(10)]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<IEnumerable<ProductImage>>> Handle(GetProductImagesQuery request, CancellationToken cancellationToken)
            {
                return new SuccessDataResult<IEnumerable<ProductImage>>(await _productImageRepository.GetListAsync());
            }
        }
    }
}