using System;
using System.Threading;
using System.Threading.Tasks;
using Moist.Application.Services.ShopServices.Commands;
using Moist.Core.Models;
using Moq;
using Xunit;
using static Xunit.Assert;

namespace Moist.Core.Tests
{
    public class InitialiseShopTests
    {
        private readonly Mock<IShopStore> _shopMock = new Mock<IShopStore>();
        private readonly CreateShopProfileCommandHandler _handler;

        public InitialiseShopTests()
        {
            _handler = new CreateShopProfileCommandHandler(_shopMock.Object);
        }

        private CreateShopProfileCommand Command(string userId) => new CreateShopProfileCommand {UserId = userId};

        [Fact]
        public async Task CreatesShopRecordForUser()
        {
            var userId = Guid.NewGuid().ToString();
            _shopMock.Setup(x => x.CreateShopForUser(userId)).ReturnsAsync(new Shop());

            var result = await _handler.Handle(Command(userId), CancellationToken.None);

            False(result.Error);
        }
    }
}