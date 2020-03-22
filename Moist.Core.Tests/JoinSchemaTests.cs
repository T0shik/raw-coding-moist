using System;
using System.Threading.Tasks;
using Moist.Application;
using Moist.Core.Models;
using Moq;
using Xunit;
using static Xunit.Assert;

namespace Moist.Core.Tests
{
    public class JoinSchemaTests
    {
        private readonly Mock<IUserManager> _userMock = new Mock<IUserManager>();
        private readonly Mock<IShopStore> _shopMock = new Mock<IShopStore>();
        private readonly Mock<IDateTime> _dateMock = new Mock<IDateTime>();
        private readonly JoinSchema _context;

        public JoinSchemaTests()
        {
            _context = new JoinSchema(_shopMock.Object, _userMock.Object, _dateMock.Object);
        }

        private static Schema Config =>
            new Schema
            {
                Goal = 6,
                Perpetual = true
            };

        [Fact]
        public Task Throws_WhenSchemaNotValid()
        {
            var currentTime = DateTime.Now;
            var config = Config;
            config.Perpetual = false;
            config.ValidSince = new DateTime(2010, 1, 1, 6, 0, 0);
            config.ValidUntil = new DateTime(2010, 1, 2, 6, 0, 0);
            _shopMock.Setup(x => x.GetSchema(1)).ReturnsAsync(config);
            _dateMock.Setup(x => x.Now).Returns(currentTime);

            return ThrowsAsync<Exception>(() => _context.Join("customer", 1));
        }

        [Fact]
        public Task Throws_WhenUserAlreadyJoinedSchema()
        {
            _shopMock.Setup(x => x.GetSchema(1)).ReturnsAsync(Config);
            _userMock.Setup(x => x.InSchemaAsync("customer", 1)).ReturnsAsync(true);
            return ThrowsAsync<Exception>(() => _context.Join("customer", 1));
        }

        [Fact]
        public async Task CreatesSchemaProgress()
        {
            var progress = new SchemaProgress
            {
                CustomerId = "customer",
                SchemaId = 1
            };
            _shopMock.Setup(x => x.GetSchema(1)).ReturnsAsync(Config);
            _userMock.Setup(x => x.InSchemaAsync("customer", 1)).ReturnsAsync(false);
            _userMock.Setup(x => x.CreateSchemaProgressAsync("customer", 1)).ReturnsAsync(progress);

            var savedProgress = await _context.Join("customer", 1);

            Equal("customer", savedProgress.CustomerId);
            Equal(1, savedProgress.SchemaId);
            Equal(0, savedProgress.Progress);
            False(savedProgress.Completed);
        }
    }
}