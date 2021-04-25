using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;

namespace Business.Handlers.Products.Queries
{
   public class GetProductsQuery:IRequest<IDataResult<IEnumerable<Product>>>
    {
        public class GetProductsQueryQueryHandler : IRequestHandler<GetProductsQuery, IDataResult<IEnumerable<Product>>>
        {
            private IProductRepository _productRepository;

            public GetProductsQueryQueryHandler(IProductRepository productRepository)
            {
                _productRepository = productRepository;
            }

            public async Task<IDataResult<IEnumerable<Product>>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
            {
                var list = _productRepository.GetListAsync();
                return new SuccessDataResult<IEnumerable<Product>>(await list);
            }
        }
    }
}
