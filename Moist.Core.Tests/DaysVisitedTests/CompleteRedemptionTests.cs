using System;
using System.Threading.Tasks;
using Moist.Application;
using Moist.Application.Services;
using Moist.Core.Code;
using Moist.Core.Exceptions;
using Moist.Core.Models;
using Moq;
using Xunit;
using static Xunit.Assert;

namespace Moist.Core.Tests.DaysVisitedTests
{
    public class CompleteRedemptionTests
    {
        private readonly Mock<ICodeStore> _codeMock = new Mock<ICodeStore>();
        private readonly Mock<IUserManager> _userMock = new Mock<IUserManager>();
        private readonly Mock<IShopStore> _shopMock = new Mock<IShopStore>();
        private readonly Mock<IDateTime> _dateMock = new Mock<IDateTime>();
        private readonly CompleteRedemption _context;

        public CompleteRedemptionTests()
        {
            _context = new CompleteRedemption(_shopMock.Object, _userMock.Object, _codeMock.Object, _dateMock.Object);
        }

        [Fact]
        public Task ThrowsWhenCodeInvalid()
        {
            var shopId = 1;
            var code = Guid.NewGuid().ToString();
            var validationResult = new RedemptionValidationResult {Success = false};
            _codeMock.Setup(x => x.ValidateRedemptionCode(shopId, code)).ReturnsAsync(validationResult);

            return ThrowsAsync<InvalidRedemptionCode>(() => _context.Complete(shopId, code));
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

            var result = await _context.Complete(shopId, code);

            True(result);
            True(progress.Completed);
            Equal(date, progress.CompletedOn);
        }
    }
}