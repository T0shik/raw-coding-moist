using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Moist.Application;
using Moist.Application.Services;
using Moist.Application.Services.User.Commands;
using Moist.Core.Code;
using Moist.Core.DateTimeInfrastructure;
using Moist.Core.Models;
using Moq;
using Xunit;
using static Xunit.Assert;

namespace Moist.Core.Tests.DaysVisitedTests
{
    public class InitiateRedemptionTests
    {
        private readonly Mock<ICodeStore> _codeMock = new Mock<ICodeStore>();
        private readonly Mock<IUserStore> _userMock = new Mock<IUserStore>();
        private readonly Mock<IShopStore> _shopMock = new Mock<IShopStore>();
        private readonly Mock<IDateTime> _dateMock = new Mock<IDateTime>();
        private readonly InitiateRedemptionCommandHandler _handler;

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
            _handler = new InitiateRedemptionCommandHandler(_userMock.Object, _shopMock.Object, _codeMock.Object,
                                                            _dateMock.Object);
        }

        private static InitiateRedemptionCommand Command(string userId = "customer", int schemaId = 1) =>
            new InitiateRedemptionCommand {UserId = userId, SchemaId = schemaId};

        [Fact]
        public async Task InitiateRedemption_ReturnsUniqueCode()
        {
            var code = Guid.NewGuid().ToString();
            _userMock.Setup(x => x.GetProgressAsync("customer", 1)).ReturnsAsync(Progress);
            _shopMock.Setup(x => x.GetSchema(1)).ReturnsAsync(Config);
            _codeMock.Setup(x => x.CreateRedemptionCode()).ReturnsAsync(code);

            var response = await _handler.Handle(Command(), CancellationToken.None);

            Equal(code, response.Data);
        }

        [Fact]
        public async Task InitiateRedemption_ThrowsWhenRequirementsNoMet()
        {
            var progress = Progress;
            progress.Progress = 5;
            _userMock.Setup(x => x.GetProgressAsync("customer", 1)).ReturnsAsync(progress);
            _shopMock.Setup(x => x.GetSchema(1)).ReturnsAsync(Config);

            var response = await _handler.Handle(Command(), CancellationToken.None);

            True(response.Error);
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

            var response = await _handler.Handle(Command(), CancellationToken.None);

            Equal(code, response.Data);
        }

        public static IEnumerable<object[]> InvalidDates =>
            new List<object[]>
            {
                new object[] {new DateTime(2010, 1, 1, 5, 59, 59, 999)},
                new object[] {new DateTime(2010, 1, 2, 6, 0, 0, 1)}
            };

        [Theory]
        [MemberData(nameof(InvalidDates))]
        public async Task InitiateRedemption_ThrowWhenOutsideValidityDate(DateTime currentTime)
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

            var response = await _handler.Handle(Command(), CancellationToken.None);

            True(response.Error);
        }
    }
}