using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moist.Application;
using Moist.Core.Code;
using Moist.Core.Models;
using Moq;
using Xunit;
using static Xunit.Assert;

namespace Moist.Core.Tests.DaysVisitedTests
{
    public class RewardCustomerTests
    {
        private readonly Mock<IUserManager> _userMock = new Mock<IUserManager>();
        private readonly Mock<IShopManager> _shopMock = new Mock<IShopManager>();
        private readonly Mock<IDateTime> _dateMock = new Mock<IDateTime>();
        private readonly Mock<ICodeGenerator> _codeMock = new Mock<ICodeGenerator>();
        private readonly RewardCustomer _context;

        private static SchemaProgress Progress =>
            new SchemaProgress
            {
                CustomerId = "customer",
                SchemaId = 1,
                Progress = 0
            };

        private static DaysVisitedSchemaConfiguration Config =>
            new DaysVisitedSchemaConfiguration
            {
                Goal = 6,
                ShopId = 1,
                Perpetual = true
            };

        public RewardCustomerTests()
        {
            _context = new RewardCustomer(_shopMock.Object, _userMock.Object, _codeMock.Object, _dateMock.Object);
        }

        [Fact]
        public async Task IncrementsProgress_WhenPerpetualAndValidCode()
        {
            var progress = Progress;
            _shopMock.Setup(x => x.GetSchema<DaysVisitedSchemaConfiguration>(1)).ReturnsAsync(Config);
            _userMock.Setup(x => x.GetProgressAsync("customer", 1))
                     .ReturnsAsync(progress);
            _codeMock.Setup(x => x.ValidateCode(1, "code")).ReturnsAsync(true);

            var success = await _context.Reward("customer", 1, "code");

            True(success);
            Equal(1, progress.Progress);
        }

        public static IEnumerable<object[]> InvalidDates =>
            new List<object[]>
            {
                new object[] {new DateTime(2010, 1, 1, 5, 59, 59, 999)},
                new object[] {new DateTime(2010, 1, 2, 6, 0, 0, 1)}
            };

        [Theory]
        [MemberData(nameof(InvalidDates))]
        public Task Throws_WhenWhenOutsideValidityDate(DateTime currentTime)
        {
            var config = Config;
            config.Perpetual = false;
            config.ValidSince = new DateTime(2010, 1, 1, 6, 0, 0);
            config.ValidUntil = new DateTime(2010, 1, 2, 6, 0, 0);
            _shopMock.Setup(x => x.GetSchema<DaysVisitedSchemaConfiguration>(1)).ReturnsAsync(config);
            _userMock.Setup(x => x.GetProgressAsync("customer", 1)).ReturnsAsync(Progress);
            _dateMock.Setup(x => x.Now).Returns(currentTime);

            return ThrowsAsync<Exception>(() => _context.Reward("customer", 1, "code"));
        }

        [Fact]
        public Task Throws_WhenInvalidCode()
        {
            _shopMock.Setup(x => x.GetSchema<DaysVisitedSchemaConfiguration>(1)).ReturnsAsync(Config);
            _userMock.Setup(x => x.GetProgressAsync("customer", 1)).ReturnsAsync(Progress);
            _codeMock.Setup(x => x.ValidateCode(1, "code")).ReturnsAsync(false);

            return ThrowsAsync<Exception>(() => _context.Reward("customer", 1, "code"));
        }
    }
}