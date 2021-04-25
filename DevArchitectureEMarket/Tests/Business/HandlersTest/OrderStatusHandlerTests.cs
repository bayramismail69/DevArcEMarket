
using Business.Handlers.OrderStatuses.Queries;
using DataAccess.Abstract;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using static Business.Handlers.OrderStatuses.Queries.GetOrderStatusQuery;
using Entities.Concrete;
using static Business.Handlers.OrderStatuses.Queries.GetOrderStatusesQuery;
using static Business.Handlers.OrderStatuses.Commands.CreateOrderStatusCommand;
using Business.Handlers.OrderStatuses.Commands;
using Business.Constants;
using static Business.Handlers.OrderStatuses.Commands.UpdateOrderStatusCommand;
using static Business.Handlers.OrderStatuses.Commands.DeleteOrderStatusCommand;
using MediatR;
using System.Linq;
using FluentAssertions;


namespace Tests.Business.HandlersTest
{
    [TestFixture]
    public class OrderStatusHandlerTests
    {
        Mock<IOrderStatusRepository> _orderStatusRepository;
        Mock<IMediator> _mediator;
        [SetUp]
        public void Setup()
        {
            _orderStatusRepository = new Mock<IOrderStatusRepository>();
            _mediator = new Mock<IMediator>();
        }

        [Test]
        public async Task OrderStatus_GetQuery_Success()
        {
            //Arrange
            var query = new GetOrderStatusQuery();

            _orderStatusRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<OrderStatus, bool>>>())).ReturnsAsync(new OrderStatus()
//propertyler buraya yazılacak
//{																		
//OrderStatusId = 1,
//OrderStatusName = "Test"
//}
);

            var handler = new GetOrderStatusQueryHandler(_orderStatusRepository.Object, _mediator.Object);

            //Act
            var x = await handler.Handle(query, new System.Threading.CancellationToken());

            //Asset
            x.Success.Should().BeTrue();
            //x.Data.OrderStatusId.Should().Be(1);

        }

        [Test]
        public async Task OrderStatus_GetQueries_Success()
        {
            //Arrange
            var query = new GetOrderStatusesQuery();

            _orderStatusRepository.Setup(x => x.GetListAsync(It.IsAny<Expression<Func<OrderStatus, bool>>>()))
                        .ReturnsAsync(new List<OrderStatus> { new OrderStatus() { /*TODO:propertyler buraya yazılacak OrderStatusId = 1, OrderStatusName = "test"*/ } });

            var handler = new GetOrderStatusesQueryHandler(_orderStatusRepository.Object, _mediator.Object);

            //Act
            var x = await handler.Handle(query, new System.Threading.CancellationToken());

            //Asset
            x.Success.Should().BeTrue();
            ((List<OrderStatus>)x.Data).Count.Should().BeGreaterThan(1);

        }

        [Test]
        public async Task OrderStatus_CreateCommand_Success()
        {
            OrderStatus rt = null;
            //Arrange
            var command = new CreateOrderStatusCommand();
            //propertyler buraya yazılacak
            //command.OrderStatusName = "deneme";

            _orderStatusRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<OrderStatus, bool>>>()))
                        .ReturnsAsync(rt);

            _orderStatusRepository.Setup(x => x.Add(It.IsAny<OrderStatus>())).Returns(new OrderStatus());

            var handler = new CreateOrderStatusCommandHandler(_orderStatusRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            _orderStatusRepository.Verify(x => x.SaveChangesAsync());
            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Added);
        }

        [Test]
        public async Task OrderStatus_CreateCommand_NameAlreadyExist()
        {
            //Arrange
            var command = new CreateOrderStatusCommand();
            //propertyler buraya yazılacak 
            //command.OrderStatusName = "test";

            _orderStatusRepository.Setup(x => x.Query())
                                           .Returns(new List<OrderStatus> { new OrderStatus() { /*TODO:propertyler buraya yazılacak OrderStatusId = 1, OrderStatusName = "test"*/ } }.AsQueryable());

            _orderStatusRepository.Setup(x => x.Add(It.IsAny<OrderStatus>())).Returns(new OrderStatus());

            var handler = new CreateOrderStatusCommandHandler(_orderStatusRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            x.Success.Should().BeFalse();
            x.Message.Should().Be(Messages.NameAlreadyExist);
        }

        [Test]
        public async Task OrderStatus_UpdateCommand_Success()
        {
            //Arrange
            var command = new UpdateOrderStatusCommand();
            //command.OrderStatusName = "test";

            _orderStatusRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<OrderStatus, bool>>>()))
                        .ReturnsAsync(new OrderStatus() { /*TODO:propertyler buraya yazılacak OrderStatusId = 1, OrderStatusName = "deneme"*/ });

            _orderStatusRepository.Setup(x => x.Update(It.IsAny<OrderStatus>())).Returns(new OrderStatus());

            var handler = new UpdateOrderStatusCommandHandler(_orderStatusRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            _orderStatusRepository.Verify(x => x.SaveChangesAsync());
            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Updated);
        }

        [Test]
        public async Task OrderStatus_DeleteCommand_Success()
        {
            //Arrange
            var command = new DeleteOrderStatusCommand();

            _orderStatusRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<OrderStatus, bool>>>()))
                        .ReturnsAsync(new OrderStatus() { /*TODO:propertyler buraya yazılacak OrderStatusId = 1, OrderStatusName = "deneme"*/});

            _orderStatusRepository.Setup(x => x.Delete(It.IsAny<OrderStatus>()));

            var handler = new DeleteOrderStatusCommandHandler(_orderStatusRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            _orderStatusRepository.Verify(x => x.SaveChangesAsync());
            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Deleted);
        }
    }
}

