
using Business.Handlers.ProductImages.Queries;
using DataAccess.Abstract;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using static Business.Handlers.ProductImages.Queries.GetProductImageQuery;
using Entities.Concrete;
using static Business.Handlers.ProductImages.Queries.GetProductImagesQuery;
using static Business.Handlers.ProductImages.Commands.CreateProductImageCommand;
using Business.Handlers.ProductImages.Commands;
using Business.Constants;
using static Business.Handlers.ProductImages.Commands.UpdateProductImageCommand;
using static Business.Handlers.ProductImages.Commands.DeleteProductImageCommand;
using MediatR;
using System.Linq;
using FluentAssertions;


namespace Tests.Business.HandlersTest
{
    [TestFixture]
    public class ProductImageHandlerTests
    {
        Mock<IProductImageRepository> _productImageRepository;
        Mock<IMediator> _mediator;
        [SetUp]
        public void Setup()
        {
            _productImageRepository = new Mock<IProductImageRepository>();
            _mediator = new Mock<IMediator>();
        }

        [Test]
        public async Task ProductImage_GetQuery_Success()
        {
            //Arrange
            var query = new GetProductImageQuery();

            _productImageRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<ProductImage, bool>>>())).ReturnsAsync(new ProductImage()
//propertyler buraya yazılacak
//{																		
//ProductImageId = 1,
//ProductImageName = "Test"
//}
);

            var handler = new GetProductImageQueryHandler(_productImageRepository.Object, _mediator.Object);

            //Act
            var x = await handler.Handle(query, new System.Threading.CancellationToken());

            //Asset
            x.Success.Should().BeTrue();
            //x.Data.ProductImageId.Should().Be(1);

        }

        [Test]
        public async Task ProductImage_GetQueries_Success()
        {
            //Arrange
            var query = new GetProductImagesQuery();

            _productImageRepository.Setup(x => x.GetListAsync(It.IsAny<Expression<Func<ProductImage, bool>>>()))
                        .ReturnsAsync(new List<ProductImage> { new ProductImage() { /*TODO:propertyler buraya yazılacak ProductImageId = 1, ProductImageName = "test"*/ } });

            var handler = new GetProductImagesQueryHandler(_productImageRepository.Object, _mediator.Object);

            //Act
            var x = await handler.Handle(query, new System.Threading.CancellationToken());

            //Asset
            x.Success.Should().BeTrue();
            ((List<ProductImage>)x.Data).Count.Should().BeGreaterThan(1);

        }

        [Test]
        public async Task ProductImage_CreateCommand_Success()
        {
            ProductImage rt = null;
            //Arrange
            var command = new CreateProductImageCommand();
            //propertyler buraya yazılacak
            //command.ProductImageName = "deneme";

            _productImageRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<ProductImage, bool>>>()))
                        .ReturnsAsync(rt);

            _productImageRepository.Setup(x => x.Add(It.IsAny<ProductImage>())).Returns(new ProductImage());

            var handler = new CreateProductImageCommandHandler(_productImageRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            _productImageRepository.Verify(x => x.SaveChangesAsync());
            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Added);
        }

        [Test]
        public async Task ProductImage_CreateCommand_NameAlreadyExist()
        {
            //Arrange
            var command = new CreateProductImageCommand();
            //propertyler buraya yazılacak 
            //command.ProductImageName = "test";

            _productImageRepository.Setup(x => x.Query())
                                           .Returns(new List<ProductImage> { new ProductImage() { /*TODO:propertyler buraya yazılacak ProductImageId = 1, ProductImageName = "test"*/ } }.AsQueryable());

            _productImageRepository.Setup(x => x.Add(It.IsAny<ProductImage>())).Returns(new ProductImage());

            var handler = new CreateProductImageCommandHandler(_productImageRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            x.Success.Should().BeFalse();
            x.Message.Should().Be(Messages.NameAlreadyExist);
        }

        [Test]
        public async Task ProductImage_UpdateCommand_Success()
        {
            //Arrange
            var command = new UpdateProductImageCommand();
            //command.ProductImageName = "test";

            _productImageRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<ProductImage, bool>>>()))
                        .ReturnsAsync(new ProductImage() { /*TODO:propertyler buraya yazılacak ProductImageId = 1, ProductImageName = "deneme"*/ });

            _productImageRepository.Setup(x => x.Update(It.IsAny<ProductImage>())).Returns(new ProductImage());

            var handler = new UpdateProductImageCommandHandler(_productImageRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            _productImageRepository.Verify(x => x.SaveChangesAsync());
            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Updated);
        }

        [Test]
        public async Task ProductImage_DeleteCommand_Success()
        {
            //Arrange
            var command = new DeleteProductImageCommand();

            _productImageRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<ProductImage, bool>>>()))
                        .ReturnsAsync(new ProductImage() { /*TODO:propertyler buraya yazılacak ProductImageId = 1, ProductImageName = "deneme"*/});

            _productImageRepository.Setup(x => x.Delete(It.IsAny<ProductImage>()));

            var handler = new DeleteProductImageCommandHandler(_productImageRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            _productImageRepository.Verify(x => x.SaveChangesAsync());
            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Deleted);
        }
    }
}

