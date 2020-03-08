﻿using System;
using System.Threading.Tasks;
using Moist.Core;
using Moq;
using Xunit;

namespace Moist.Configuration.Tests
{
    public class InitialiseShopTests
    {
        private readonly Mock<IShopStore> _shopMock = new Mock<IShopStore>();

        [Fact]
        public async Task CreatesShopRecordForUser()
        {
            var userId = Guid.NewGuid().ToString();
            var context = new InitialiseShopContext(_shopMock.Object);
            _shopMock.Setup(x => x.CreateShopForUser(userId)).ReturnsAsync(true);

            var result = await context.Initialise(userId);
            
            Assert.True(result);
        }
    }

    public class InitialiseShopContext
    {
        private readonly IShopStore _shopStore;

        public InitialiseShopContext(IShopStore shopStore)
        {
            _shopStore = shopStore;
        }

        public Task<bool> Initialise(string userId)
        {
            return _shopStore.CreateShopForUser(userId);
        }
    }
}