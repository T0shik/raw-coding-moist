using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Moist.Application.Services.ShopServices.Commands;
using Moist.Core.Code;
using Moist.Core.DateTimeInfrastructure;
using Moist.Core.Models;
using Moq;
using Xunit;
using static Xunit.Assert;

namespace Moist.Core.Tests.DaysVisitedTests
{
    public class RewardCustomerTests
    {
        private readonly Mock<IUserStore> _userMock = new Mock<IUserStore>();
        private readonly Mock<IShopStore> _shopMock = new Mock<IShopStore>();
        private readonly Mock<IDateTime> _dateMock = new Mock<IDateTime>();
        private readonly Mock<ICodeStore> _codeMock = new Mock<ICodeStore>();
        private readonly RewardCustomerCommandHandler _context;

        private static SchemaProgress Progress =>
            new SchemaProgress
            {
                CustomerId = "customer",
                SchemaId = 1,
                Progress = 0
            };

        private static Schema Config =>
            new Schema
            {
                Goal = 6,
                ShopId = 1,
                Perpetual = true
            };

        private static RewardCustomerCommand Command(
            string userId = "customer",
            int schemaId = 1,
            string code = "code") =>
            new RewardCustomerCommand {UserId = userId, SchemaId = schemaId, Code = code};

        public RewardCustomerTests()
        {
            _context = new RewardCustomerCommandHandler(_shopMock.Object, _userMock.Object, _codeMock.Object,
                                                        _dateMock.Object);
        }

        [Fact]
        public async Task IncrementsProgress_WhenPerpetualAndValidCode()
        {
            var progress = Progress;
            _shopMock.Setup(x => x.GetSchema(1)).ReturnsAsync(Config);
            _userMock.Setup(x => x.GetProgressAsync("customer", 1))
                     .ReturnsAsync(progress);
            _codeMock.Setup(x => x.ValidateRewardCode(1, "code")).ReturnsAsync(new ValidationResult {Success = true});

            var response = await _context.Handle(Command(), CancellationToken.None);

            False(response.Error);
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
        public async Task Throws_WhenWhenOutsideValidityDate(DateTime currentTime)
        {
            var config = Config;
            config.Perpetual = false;
            config.ValidSince = new DateTime(2010, 1, 1, 6, 0, 0);
            config.ValidUntil = new DateTime(2010, 1, 2, 6, 0, 0);
            _shopMock.Setup(x => x.GetSchema(1)).ReturnsAsync(config);
            _userMock.Setup(x => x.GetProgressAsync("customer", 1)).ReturnsAsync(Progress);
            _dateMock.Setup(x => x.Now).Returns(currentTime);

            var response = await _context.Handle(Command(), CancellationToken.None);

            True(response.Error);
        }

        [Fact]
        public async Task Throws_WhenInvalidCode()
        {
            _shopMock.Setup(x => x.GetSchema(1)).ReturnsAsync(Config);
            _userMock.Setup(x => x.GetProgressAsync("customer", 1)).ReturnsAsync(Progress);
            _codeMock.Setup(x => x.ValidateRewardCode(1, "code")).ReturnsAsync(new ValidationResult {Success = false});

            var response = await _context.Handle(Command(), CancellationToken.None);

            True(response.Error);
        }
    }
}