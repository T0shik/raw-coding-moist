using System;
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
    public class CompleteRedemptionTests
    {
        private readonly Mock<ICodeStore> _codeMock = new Mock<ICodeStore>();
        private readonly Mock<IUserStore> _userMock = new Mock<IUserStore>();
        private readonly Mock<IDateTime> _dateMock = new Mock<IDateTime>();
        private readonly CompleteRedemptionCommandHandler _handler;

        public CompleteRedemptionTests()
        {
            _handler = new CompleteRedemptionCommandHandler(_userMock.Object, _codeMock.Object, _dateMock.Object);
        }

        private CompleteRedemptionCommand Command(int shopId, string code) =>
            new CompleteRedemptionCommand
            {
                ShopId = shopId,
                Code = code
            };

        [Fact]
        public async Task ThrowsWhenCodeInvalid()
        {
            var shopId = 1;
            var code = Guid.NewGuid().ToString();
            var validationResult = new RedemptionValidationResult {Success = false};
            _codeMock.Setup(x => x.ValidateRedemptionCode(shopId, code)).ReturnsAsync(validationResult);

            var response = await _handler.Handle(Command(shopId, code), CancellationToken.None);

            True(response.Error);
        }

        [Fact]
        public async Task CompletesSchemaProgressWhenValidCode()
        {
            var progress = new SchemaProgress
            {
                CustomerId = "customer",
                SchemaId = 1,
                Progress = 6
            };

            var shopId = 1;
            var code = Guid.NewGuid().ToString();
            var validationResult = new RedemptionValidationResult {Success = true, UserId = "customer", ProgressId = 1};
            var date = DateTime.Now;
            _codeMock.Setup(x => x.ValidateRedemptionCode(shopId, code)).ReturnsAsync(validationResult);
            _userMock.Setup(x => x.GetProgressAsync("customer", 1)).ReturnsAsync(progress);
            _dateMock.Setup(x => x.Now).Returns(date);

            var result = await _handler.Handle(Command(shopId, code), CancellationToken.None);

            False(result.Error);
            True(progress.Completed);
            Equal(date, progress.CompletedOn);
        }
    }
}