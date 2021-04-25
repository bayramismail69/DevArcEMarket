
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
using Business.Handlers.ProductImages.ValidationRules;

namespace Business.Handlers.ProductImages.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateProductImageCommand : IRequest<IResult>
    {

        public int ProductId { get; set; }
        public string ImagePath { get; set; }
        public System.DateTime DateOfUpload { get; set; }


        public class CreateProductImageCommandHandler : IRequestHandler<CreateProductImageCommand, IResult>
        {
            private readonly IProductImageRepository _productImageRepository;
            private readonly IMediator _mediator;
            public CreateProductImageCommandHandler(IProductImageRepository productImageRepository, IMediator mediator)
            {
                _productImageRepository = productImageRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(CreateProductImageValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(CreateProductImageCommand request, CancellationToken cancellationToken)
            {
                var isThereProductImageRecord = _productImageRepository.Query().Any(u => u.ProductId == request.ProductId);

                if (isThereProductImageRecord == true)
                    return new ErrorResult(Messages.NameAlreadyExist);

                var addedProductImage = new ProductImage
                {
                    ProductId = request.ProductId,
                    ImagePath = request.ImagePath,
                    DateOfUpload = request.DateOfUpload,

                };

                _productImageRepository.Add(addedProductImage);
                await _productImageRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Added);
            }
        }
    }
}