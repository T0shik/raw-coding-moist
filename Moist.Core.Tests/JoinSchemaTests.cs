using System;
using System.Threading;
using System.Threading.Tasks;
using Moist.Application.Services.Users.Commands;
using Moist.Core.DateTimeInfrastructure;
using Moist.Core.Models;
using Moq;
using Xunit;
using static Xunit.Assert;

namespace Moist.Core.Tests
{
    public class JoinSchemaTests
    {
        private readonly Mock<IUserStore> _userMock = new Mock<IUserStore>();
        private readonly Mock<IShopStore> _shopMock = new Mock<IShopStore>();
        private readonly Mock<IDateTime> _dateMock = new Mock<IDateTime>();
        private readonly JoinSchemaCommandHandler _handler;

        public JoinSchemaTests()
        {
            _handler = new JoinSchemaCommandHandler(_shopMock.Object, _userMock.Object, _dateMock.Object);
        }

        private static Schema Config =>
            new Schema
            {
                Goal = 6,
                Type = SchemaType.DaysVisited,
                Perpetual = true
            };

        private static JoinSchemaCommand Command(string userId = "customer", int schemaId = 1) =>
            new JoinSchemaCommand {UserId = userId, SchemaId = schemaId};

        [Fact]
        public async Task Throws_WhenSchemaNotValid()
        {
            var currentTime = DateTime.Now;
            var config = Config;
            config.Perpetual = false;
            config.ValidSince = new DateTime(2010, 1, 1, 6, 0, 0);
            config.ValidUntil = new DateTime(2010, 1, 2, 6, 0, 0);
            _shopMock.Setup(x => x.GetSchema(1)).ReturnsAsync(config);
            _dateMock.Setup(x => x.Now).Returns(currentTime);

            var response = await _handler.Handle(Command(), CancellationToken.None);

            True(response.Error);
        }

        [Fact]
        public async Task FalseResponse_WhenUserAlreadyJoinedSchema()
        {
            _shopMock.Setup(x => x.GetSchema(1)).ReturnsAsync(Config);
            _userMock.Setup(x => x.InSchemaAsync("customer", 1)).ReturnsAsync(true);

            var response = await _handler.Handle(Command(), CancellationToken.None);

            True(response.Error);
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

            var response = await _handler.Handle(Command(), CancellationToken.None);
            var savedProgress = response.Data;

            Equal("customer", savedProgress.CustomerId);
            Equal(1, savedProgress.SchemaId);
            Equal(0, savedProgress.Progress);
            False(savedProgress.Completed);
        }
    }
}