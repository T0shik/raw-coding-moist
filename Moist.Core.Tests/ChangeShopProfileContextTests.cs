using System;
using System.Threading.Tasks;
using Moist.Application;
using Moist.Application.Services;
using Moist.Core.Models;
using Moq;
using Xunit;
using static Xunit.Assert;

namespace Moist.Core.Tests
{
    public class ChangeShopProfileContextTests
    {
        private readonly Mock<IShopStore> _shopMock = new Mock<IShopStore>();
        private readonly ChangeShopProfileContext _context;
        private const int c_shopId = 1;
        private const string c_userId = "userId";

        private static ChangeShopProfileContext.Form NewForm =>
            new ChangeShopProfileContext.Form
            {
                UserId = c_userId,
                Id = c_shopId,
                Name = "Shop Name",
                Description = "Shop Description"
            };

        public ChangeShopProfileContextTests()
        {
            _context = new ChangeShopProfileContext(_shopMock.Object);
        }

        [Fact]
        public async Task ReturnsFalseWhen_InvalidUser()
        {
            var userId = Guid.NewGuid().ToString();
            _shopMock.Setup(x => x.UserCanChangeProfile(userId, c_shopId)).ReturnsAsync(false);

            var result = await _context.Change(NewForm);

            False(result);
        }

        [Fact]
        public async Task AssignsFormPropertiesToShop()
        {
            var mock = new Shop {Id = c_shopId};
            _shopMock.Setup(x => x.GetProfile(c_shopId)).ReturnsAsync(mock);
            _shopMock.Setup(x => x.UserCanChangeProfile(c_userId, c_shopId)).ReturnsAsync(true);

            var result = await _context.Change(NewForm);

            True(result);
            Equal(NewForm.Name, mock.Name);
            Equal(NewForm.Description, mock.Description);
        }
    }
}