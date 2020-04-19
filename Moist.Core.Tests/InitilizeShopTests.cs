using System;
using System.Threading;
using System.Threading.Tasks;
using Moist.Application;
using Moist.Application.Services;
using Moist.Application.Services.Shop.Commands;
using Moq;
using Xunit;
using static Xunit.Assert;

namespace Moist.Core.Tests
{
    public class InitialiseShopTests
    {
        private readonly Mock<IShopStore> _shopMock = new Mock<IShopStore>();
        private readonly InitialiseShopCommandHandler _handler;

        public InitialiseShopTests()
        {
            _handler = new InitialiseShopCommandHandler(_shopMock.Object);
        }

        private InitialiseShopCommand Command(string userId) => new InitialiseShopCommand {UserId = userId};

        [Fact]
        public async Task CreatesShopRecordForUser()
        {
            var userId = Guid.NewGuid().ToString();
            _shopMock.Setup(x => x.CreateShopForUser(userId)).ReturnsAsync(true);

            var result = await _handler.Handle(Command(userId), CancellationToken.None);

            False(result.Error);
        }
    }
}