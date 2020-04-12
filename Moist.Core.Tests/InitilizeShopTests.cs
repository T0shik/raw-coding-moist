using System;
using System.Threading.Tasks;
using Moist.Application;
using Moist.Application.Services;
using Moq;
using Xunit;

namespace Moist.Core.Tests
{
    public class InitialiseShopTests
    {
        private readonly Mock<IShopStore> _shopMock = new Mock<IShopStore>();

        [Fact]
        public async Task CreatesShopRecordForUser()
        {
            var userId = Guid.NewGuid().ToString();
            var context = new InitialiseShopContext(_shopMock.Object);
            _shopMock.Setup(x => x.CreateShopForUser(userId)).ReturnsAsync(true);

            var result = await context.Initialise(userId);
            
            Assert.True(result);
        }
    }
}