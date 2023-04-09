using Core.Features.Queries.Customers;
using Core.Repository.Interfaces;
using MapsterMapper;
using Moq;

namespace PropertyManager.Core.Tests.Features.Commands.Customer
{
    public class GetAllCustomerQueryHandlerTests
    { 
        [Fact]
        public void GetAllCustomerQueryHandler_Should_ReturnCustomers_When_Successful()
        {
            //Arrange
            var mapper = new Mock<IMapper>();
            var repo = new Mock<ICustomerRepository>();
            var cache = new Mock<ICache>();

            var request = new GetAllCustomersQuery();
            var sut = new GetAllCustomersQueryHandler(repo.Object, mapper.Object, cache.Object);

            //Act
            var result = sut.Handle(request, default);

            //Assert
            Assert.NotNull(result);
        }
    }
}