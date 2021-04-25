
using Business.Handlers.Countries.Queries;
using DataAccess.Abstract;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using static Business.Handlers.Countries.Queries.GetCountryQuery;
using Entities.Concrete;
using static Business.Handlers.Countries.Queries.GetCountriesQuery;
using static Business.Handlers.Countries.Commands.CreateCountryCommand;
using Business.Handlers.Countries.Commands;
using Business.Constants;
using static Business.Handlers.Countries.Commands.UpdateCountryCommand;
using static Business.Handlers.Countries.Commands.DeleteCountryCommand;
using MediatR;
using System.Linq;
using FluentAssertions;


namespace Tests.Business.HandlersTest
{
    [TestFixture]
    public class CountryHandlerTests
    {
        Mock<ICountryRepository> _countryRepository;
        Mock<IMediator> _mediator;
        [SetUp]
        public void Setup()
        {
            _countryRepository = new Mock<ICountryRepository>();
            _mediator = new Mock<IMediator>();
        }

        [Test]
        public async Task Country_GetQuery_Success()
        {
            //Arrange
            var query = new GetCountryQuery();

            _countryRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Country, bool>>>())).ReturnsAsync(new Country()
//propertyler buraya yazılacak
//{																		
//CountryId = 1,
//CountryName = "Test"
//}
);

            var handler = new GetCountryQueryHandler(_countryRepository.Object, _mediator.Object);

            //Act
            var x = await handler.Handle(query, new System.Threading.CancellationToken());

            //Asset
            x.Success.Should().BeTrue();
            //x.Data.CountryId.Should().Be(1);

        }

        [Test]
        public async Task Country_GetQueries_Success()
        {
            //Arrange
            var query = new GetCountriesQuery();

            _countryRepository.Setup(x => x.GetListAsync(It.IsAny<Expression<Func<Country, bool>>>()))
                        .ReturnsAsync(new List<Country> { new Country() { /*TODO:propertyler buraya yazılacak CountryId = 1, CountryName = "test"*/ } });

            var handler = new GetCountriesQueryHandler(_countryRepository.Object, _mediator.Object);

            //Act
            var x = await handler.Handle(query, new System.Threading.CancellationToken());

            //Asset
            x.Success.Should().BeTrue();
            ((List<Country>)x.Data).Count.Should().BeGreaterThan(1);

        }

        [Test]
        public async Task Country_CreateCommand_Success()
        {
            Country rt = null;
            //Arrange
            var command = new CreateCountryCommand();
            //propertyler buraya yazılacak
            //command.CountryName = "deneme";

            _countryRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Country, bool>>>()))
                        .ReturnsAsync(rt);

            _countryRepository.Setup(x => x.Add(It.IsAny<Country>())).Returns(new Country());

            var handler = new CreateCountryCommandHandler(_countryRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            _countryRepository.Verify(x => x.SaveChangesAsync());
            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Added);
        }

        [Test]
        public async Task Country_CreateCommand_NameAlreadyExist()
        {
            //Arrange
            var command = new CreateCountryCommand();
            //propertyler buraya yazılacak 
            //command.CountryName = "test";

            _countryRepository.Setup(x => x.Query())
                                           .Returns(new List<Country> { new Country() { /*TODO:propertyler buraya yazılacak CountryId = 1, CountryName = "test"*/ } }.AsQueryable());

            _countryRepository.Setup(x => x.Add(It.IsAny<Country>())).Returns(new Country());

            var handler = new CreateCountryCommandHandler(_countryRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            x.Success.Should().BeFalse();
            x.Message.Should().Be(Messages.NameAlreadyExist);
        }

        [Test]
        public async Task Country_UpdateCommand_Success()
        {
            //Arrange
            var command = new UpdateCountryCommand();
            //command.CountryName = "test";

            _countryRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Country, bool>>>()))
                        .ReturnsAsync(new Country() { /*TODO:propertyler buraya yazılacak CountryId = 1, CountryName = "deneme"*/ });

            _countryRepository.Setup(x => x.Update(It.IsAny<Country>())).Returns(new Country());

            var handler = new UpdateCountryCommandHandler(_countryRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            _countryRepository.Verify(x => x.SaveChangesAsync());
            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Updated);
        }

        [Test]
        public async Task Country_DeleteCommand_Success()
        {
            //Arrange
            var command = new DeleteCountryCommand();

            _countryRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Country, bool>>>()))
                        .ReturnsAsync(new Country() { /*TODO:propertyler buraya yazılacak CountryId = 1, CountryName = "deneme"*/});

            _countryRepository.Setup(x => x.Delete(It.IsAny<Country>()));

            var handler = new DeleteCountryCommandHandler(_countryRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            _countryRepository.Verify(x => x.SaveChangesAsync());
            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Deleted);
        }
    }
}

