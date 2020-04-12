using System.Threading.Tasks;
using Moist.Application;
using Moist.Application.Services;
using Moist.Core.Models;
using Moq;
using Xunit;
using static Xunit.Assert;

namespace Moist.Core.Tests
{
    public class CloseSchemaTests
    {
        private readonly Mock<IShopStore> _shopMock = new Mock<IShopStore>();
        private readonly CloseSchemaContext _context;
        private const int c_shopId = 1;
        private const int c_schemaId = 1;

        private Schema CreateMockSchema() =>
            new Schema
            {
                Id = c_schemaId,
                Activate = true
            };

        public CloseSchemaTests()
        {
            _context = new CloseSchemaContext(_shopMock.Object);
        }

        [Fact]
        public async Task SetsEnableFlagToFalse_WhenSchemaBelongsToShop()
        {
            var mockSchema = CreateMockSchema();
            mockSchema.ShopId = c_shopId;
            _shopMock.Setup(x => x.GetSchema(c_schemaId)).ReturnsAsync(mockSchema);
            
            var result = await _context.Close(c_shopId, c_schemaId);
            
            True(result);
            True(mockSchema.Closed);
            False(mockSchema.Activate);
        }
        
        [Fact]
        public async Task ReturnsFalseWhenSchemaNotOwnedByShop()
        {
            var mockSchema = CreateMockSchema();
            _shopMock.Setup(x => x.GetSchema(c_schemaId)).ReturnsAsync(mockSchema);
            
            var result = await _context.Close(2, c_schemaId);
            
            False(result);
            True(mockSchema.Activate);
        }
    }
}