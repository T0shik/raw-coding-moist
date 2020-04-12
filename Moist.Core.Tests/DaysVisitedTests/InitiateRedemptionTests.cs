using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moist.Application;
using Moist.Application.Services;
using Moist.Core.Code;
using Moist.Core.Models;
using Moq;
using Xunit;

namespace Moist.Core.Tests.DaysVisitedTests
{
    public class InitiateRedemptionTests
    {
        private readonly Mock<ICodeStore> _codeMock = new Mock<ICodeStore>();
        private readonly Mock<IUserManager> _userMock = new Mock<IUserManager>();
        private readonly Mock<IShopStore> _shopMock = new Mock<IShopStore>();
        private readonly Mock<IDateTime> _dateMock = new Mock<IDateTime>();
        private readonly InitiateRedemption _context;

        private static SchemaProgress Progress =>
            new SchemaProgress
            {
                CustomerId = "customer",
                SchemaId = 1,
                Progress = 6
            };

        private static Schema Config =>
            new Schema
            {
                Goal = 6,
                Perpetual = true
            };
        
        public InitiateRedemptionTests()
        {
            _context = new InitiateRedemption(_userMock.Object, _shopMock.Object, _codeMock.Object, _dateMock.Object);
        }
        
        [Fact]
        public async Task InitiateRedemption_ReturnsUniqueCode()
        {
            var code = Guid.NewGuid().ToString();
            _userMock.Setup(x => x.GetProgressAsync("customer", 1)).ReturnsAsync(Progress);
            _shopMock.Setup(x => x.GetSchema(1)).ReturnsAsync(Config);
            _codeMock.Setup(x => x.CreateRedemptionCode()).ReturnsAsync(code);

            var generatedCode = await _context.Redeem("customer", 1);

            Assert.Equal(code, generatedCode);
        }
        
          [Fact]
        public Task InitiateRedemption_ThrowsWhenRequirementsNoMet()
        {
            var progress = Progress;
            progress.Progress = 5;
            _userMock.Setup(x => x.GetProgressAsync("customer", 1)).ReturnsAsync(progress);
            _shopMock.Setup(x => x.GetSchema(1)).ReturnsAsync(Config);

            return Assert.ThrowsAsync<Exception>(() => _context.Redeem("customer", 1));
        }

        [Fact]
        public async Task InitiateRedemption_DontCheckDateValidityWhenPerpetual()
        {
            var config = Config;
            config.Perpetual = true;
            var code = Guid.NewGuid().ToString();
            _userMock.Setup(x => x.GetProgressAsync("customer", 1)).ReturnsAsync(Progress);
            _shopMock.Setup(x => x.GetSchema(1)).ReturnsAsync(config);
            _codeMock.Setup(x => x.CreateRedemptionCode()).ReturnsAsync(code);

            var result = await _context.Redeem("customer", 1);

            Assert.Equal(code, result);
        }

        public static IEnumerable<object[]> InvalidDates =>
            new List<object[]>
            {
                new object[] {new DateTime(2010, 1, 1, 5, 59, 59, 999)},
                new object[] {new DateTime(2010, 1, 2, 6, 0, 0, 1)}
            };

        [Theory]
        [MemberData(nameof(InvalidDates))]
        public Task InitiateRedemption_ThrowWhenOutsideValidityDate(DateTime currentTime)
        {
            var config = Config;
            config.Perpetual = false;
            config.ValidSince = new DateTime(2010, 1, 1, 6, 0, 0);
            config.ValidUntil = new DateTime(2010, 1, 2, 6, 0, 0);
            var code = Guid.NewGuid().ToString();
            _userMock.Setup(x => x.GetProgressAsync("customer", 1)).ReturnsAsync(Progress);
            _shopMock.Setup(x => x.GetSchema(1)).ReturnsAsync(config);
            _codeMock.Setup(x => x.CreateRedemptionCode()).ReturnsAsync(code);
            _dateMock.Setup(x => x.Now).Returns(currentTime);

            return Assert.ThrowsAsync<Exception>(() => _context.Redeem("customer", 1));
        }
    }
}