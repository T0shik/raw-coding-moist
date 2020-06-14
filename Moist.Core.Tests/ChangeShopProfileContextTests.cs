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
    public class ChangeShopProfileContextTests
    {
        private readonly Mock<IShopStore> _shopMock = new Mock<IShopStore>();
        private readonly ChangeShopProfileCommandHandler _handler;
        private const int c_shopId = 1;
        private const string c_userId = "userId";


        public ChangeShopProfileContextTests()
        {
            _handler = new ChangeShopProfileCommandHandler(_shopMock.Object);
        }

        private static ChangeShopProfileCommand NewForm =>
            new ChangeShopProfileCommand
            {
                UserId = c_userId,
                ShopId = c_shopId,
                Name = "Shop Name",
                Description = "Shop Description"
            };

        [Fact]
        public async Task ReturnsFalseWhen_InvalidUser()
        {
            var userId = Guid.NewGuid().ToString();
            _shopMock.Setup(x => x.UserCanChangeProfile(userId, c_shopId)).ReturnsAsync(false);

            var response = await _handler.Handle(NewForm, CancellationToken.None);

            True(response.Error);
        }

        [Fact]
        public async Task AssignsFormPropertiesToShop()
        {
            var mock = new Shop {Id = c_shopId};
            _shopMock.Setup(x => x.GetProfile(c_shopId)).ReturnsAsync(mock);
            _shopMock.Setup(x => x.UserCanChangeProfile(c_userId, c_shopId)).ReturnsAsync(true);

            var result = await _handler.Handle(NewForm, CancellationToken.None);

            False(result.Error);
            Equal(NewForm.Name, mock.Name);
            Equal(NewForm.Description, mock.Description);
        }
    }
}