using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moist.Core.Exceptions;
using Moist.Core.Models;
using Moq;
using Xunit;

namespace Moist.Core.Tests
{
    public class DaysVisitedSchemaTests
    {
        private readonly Mock<ICodeGenerator> _codeMock = new Mock<ICodeGenerator>();
        private readonly Mock<IUserManager> _userMock = new Mock<IUserManager>();
        private readonly Mock<IShopManager> _shopMock = new Mock<IShopManager>();
        private readonly Mock<IDateTime> _dateMock = new Mock<IDateTime>();
        private readonly DaysVisitedSchema _schema;

        private static SchemaProgress Progress =>
            new SchemaProgress
            {
                CustomerId = "customer",
                SchemaId = 1,
                Progress = 6
            };

        private static DaysVisitedSchemaConfiguration Config =>
            new DaysVisitedSchemaConfiguration
            {
                Goal = 6,
                Perpetual = true
            };

        public DaysVisitedSchemaTests()
        {
            _schema = new DaysVisitedSchema(_userMock.Object, _shopMock.Object, _codeMock.Object, _dateMock.Object);
        }

        [Fact]
        public void Reward_IncrementsCounterBy1()
        {
            _schema.Reward();

            Assert.Equal(1, _schema.Counter);
        }

        [Fact]
        public async Task InitiateRedemption_ReturnsUniqueCode()
        {
            var code = Guid.NewGuid().ToString();
            _userMock.Setup(x => x.GetProgressAsync("customer", 1)).ReturnsAsync(Progress);
            _shopMock.Setup(x => x.GetSchema<DaysVisitedSchemaConfiguration>(1)).ReturnsAsync(Config);
            _codeMock.Setup(x => x.CreateRedemptionCode()).ReturnsAsync(code);

            var generatedCode = await _schema.InitiateRedemption("customer", 1);

            Assert.Equal(code, generatedCode);
        }

        [Fact]
        public Task CompleteRedemption_ThrowsWhenCodeInvalid()
        {
            var code = Guid.NewGuid().ToString();
            _codeMock.Setup(x => x.ValidateCode(code)).Throws<InvalidRedemptionCode>();

            return Assert.ThrowsAsync<InvalidRedemptionCode>(() => _schema.CompleteRedemption(code));
        }

        [Fact]
        public async Task CompleteRedemption_ReturnsTrueWhenCodeIsValid()
        {
            var code = Guid.NewGuid().ToString();
            _codeMock.Setup(x => x.ValidateCode(code)).ReturnsAsync(true);

            var result = await _schema.CompleteRedemption(code);

            Assert.True(result);
        }

        [Fact]
        public Task InitiateRedemption_ThrowsWhenRequirementsNoMet()
        {
            var progress = Progress;
            progress.Progress = 5;
            _userMock.Setup(x => x.GetProgressAsync("customer", 1)).ReturnsAsync(progress);
            _shopMock.Setup(x => x.GetSchema<DaysVisitedSchemaConfiguration>(1)).ReturnsAsync(Config);

            return Assert.ThrowsAsync<Exception>(() => _schema.InitiateRedemption("customer", 1));
        }

        [Fact]
        public async Task InitiateRedemption_DontCheckDateValidityWhenPerpetual()
        {
            var config = Config;
            config.Perpetual = true;
            var code = Guid.NewGuid().ToString();
            _userMock.Setup(x => x.GetProgressAsync("customer", 1)).ReturnsAsync(Progress);
            _shopMock.Setup(x => x.GetSchema<DaysVisitedSchemaConfiguration>(1)).ReturnsAsync(config);
            _codeMock.Setup(x => x.CreateRedemptionCode()).ReturnsAsync(code);

            var result = await _schema.InitiateRedemption("customer", 1);

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
            _shopMock.Setup(x => x.GetSchema<DaysVisitedSchemaConfiguration>(1)).ReturnsAsync(config);
            _codeMock.Setup(x => x.CreateRedemptionCode()).ReturnsAsync(code);
            _dateMock.Setup(x => x.Now).Returns(currentTime);

            return Assert.ThrowsAsync<Exception>(() => _schema.InitiateRedemption("customer", 1));
        }
    }
}