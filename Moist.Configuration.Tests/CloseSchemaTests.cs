using System.Threading.Tasks;
using Moist.Configuration.Forms;
using Moist.Configuration.Tests.Mocks;
using Moist.Core;
using Moist.Core.Models;
using Moq;
using Xunit;
using static Xunit.Assert;

namespace Moist.Configuration.Tests
{
    public class CloseSchemaTests
    {
        private readonly Mock<IShopStore> _shopMock = new Mock<IShopStore>();
        private readonly CloseSchemaContext _context;
        private const int c_shopId = 1;
        private const int c_schemaId = 1;

        private MockSchema CreateMockSchema() =>
            new MockSchema
            {
                Id = c_schemaId,
                ShopId = c_shopId,
                Enabled = true
            };

        public CloseSchemaTests()
        {
            _context = new CloseSchemaContext(_shopMock.Object);
        }

        [Fact]
        public async Task SetsEnableFlagToFalse_WhenSchemaBelongsToShop()
        {
            var mockSchema = CreateMockSchema();
            _shopMock.Setup(x => x.GetSchema<BaseSchema>(c_schemaId)).ReturnsAsync(mockSchema);
            
            var result = await _context.Close(c_shopId, c_schemaId);
            
            True(result);
            False(mockSchema.Enabled);
        }
        
        [Fact]
        public async Task ReturnsFalseWhenSchemaNotOwnedByShop()
        {
            var mockSchema = CreateMockSchema();
            _shopMock.Setup(x => x.GetSchema<BaseSchema>(c_schemaId)).ReturnsAsync(mockSchema);
            
            var result = await _context.Close(2, c_schemaId);
            
            False(result);
            True(mockSchema.Enabled);
        }
    }
}