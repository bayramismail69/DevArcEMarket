
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
using Business.Handlers.ProductImages.ValidationRules;


namespace Business.Handlers.ProductImages.Commands
{


    public class UpdateProductImageCommand : IRequest<IResult>
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ImagePath { get; set; }
        public System.DateTime DateOfUpload { get; set; }

        public class UpdateProductImageCommandHandler : IRequestHandler<UpdateProductImageCommand, IResult>
        {
            private readonly IProductImageRepository _productImageRepository;
            private readonly IMediator _mediator;

            public UpdateProductImageCommandHandler(IProductImageRepository productImageRepository, IMediator mediator)
            {
                _productImageRepository = productImageRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(UpdateProductImageValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(UpdateProductImageCommand request, CancellationToken cancellationToken)
            {
                var isThereProductImageRecord = await _productImageRepository.GetAsync(u => u.Id == request.Id);


                isThereProductImageRecord.ProductId = request.ProductId;
                isThereProductImageRecord.ImagePath = request.ImagePath;
                isThereProductImageRecord.DateOfUpload = request.DateOfUpload;


                _productImageRepository.Update(isThereProductImageRecord);
                await _productImageRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Updated);
            }
        }
    }
}

