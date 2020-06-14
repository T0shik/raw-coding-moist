using System.Threading;
using System.Threading.Tasks;
using Moist.Application.Services.Schemas.Commands;
using Moist.Core.Models;
using Moq;
using Xunit;
using static Xunit.Assert;

namespace Moist.Core.Tests
{
    public class CloseSchemaTests
    {
        private readonly Mock<IShopStore> _shopMock = new Mock<IShopStore>();
        private readonly CloseSchemaCommandHandler _handler;
        private const int c_shopId = 1;
        private const int c_schemaId = 1;

        public CloseSchemaTests()
        {
            _handler = new CloseSchemaCommandHandler(_shopMock.Object);
        }

        private Schema CreateMockSchema() =>
            new Schema
            {
                Id = c_schemaId,
                Activate = true
            };

        private CloseSchemaCommand Command => new CloseSchemaCommand {ShopId = c_shopId, SchemaId = c_schemaId};

        [Fact]
        public async Task SetsEnableFlagToFalse_WhenSchemaBelongsToShop()
        {
            var mockSchema = CreateMockSchema();
            mockSchema.ShopId = c_shopId;
            _shopMock.Setup(x => x.GetSchema(c_schemaId)).ReturnsAsync(mockSchema);

            var result = await _handler.Handle(Command, CancellationToken.None);

            False(result.Error);
            True(mockSchema.Closed);
            False(mockSchema.Activate);
        }

        [Fact]
        public async Task ReturnsFalseWhenSchemaNotOwnedByShop()
        {
            var mockSchema = CreateMockSchema();
            _shopMock.Setup(x => x.GetSchema(c_schemaId)).ReturnsAsync(mockSchema);
            var result = await _handler.Handle(Command, CancellationToken.None);
            True(result.Error);
            True(mockSchema.Activate);
        }
    }
}