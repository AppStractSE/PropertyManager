using Core.Features.Commands.Customer;
using Core.Repository.Interfaces;
using MapsterMapper;
using Moq;

namespace PropertyManager.Core.Tests.Features.Commands.Customer
{
    public class AddCustomerCommandHandlerTests
    {
        [Fact]
        public void AddCustomerCommandHandler_Should_AddCustomers_When_Successful()
        {
            //Arrange
            var mapper = new Mock<IMapper>();
            var repo = new Mock<ICustomerRepository>();
            var cache = new Mock<ICache>();

            var request = new AddCustomerCommand();
            var sut = new AddCustomerCommandHandler(repo.Object, mapper.Object, cache.Object);

            //Act
            var result = sut.Handle(request, default);

            //Assert
            Assert.NotNull(result);
        }
    }
}